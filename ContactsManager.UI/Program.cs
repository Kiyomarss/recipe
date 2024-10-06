using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repositories;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using CRUDExample;
using CRUDExample.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

 loggerConfiguration
 .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
 .ReadFrom.Services(services); //read out current app's services and make them available to serilog
} );

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
 options.AddPolicy("CorsPolicy", builder =>
 {
  builder.AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader();
 });
});

var app = builder.Build();


//create application pipeline
if (builder.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
}
else
{
 app.UseExceptionHandler("/Error");
 app.UseExceptionHandlingMiddleware();
}

app.UseSerilogRequestLogging();


app.UseHttpLogging();

if (builder.Environment.IsEnvironment("Test") == false)
 Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();


app.UseRouting(); //Identifying action method based on route
app.UseCors("CorsPolicy");
app.UseAuthentication(); //Reading Identity cookie
app.UseAuthorization(); //Validates access permissions of the user
app.MapControllers(); //Execute the filter pipiline (action + filters)

app.UseEndpoints(endpoints =>
{
 endpoints.MapControllers();
});


app.UseEndpoints(endpoints =>
{
 endpoints.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}");
 
 endpoints.MapControllerRoute(
  name: "default",
  pattern: "{controller}/{action=Index}/{id?}"
  );
    
 endpoints.MapFallbackToFile("index.html"); // This serves your frontend app

 endpoints.Map("/api/{**path}", async context =>
 {
  // Your backend url
  var backendUrl = $"http://localhost:5000{context.Request.Path}";

  // Create a new HTTP request to the backend
  var requestMessage = new HttpRequestMessage();
  requestMessage.Method = new HttpMethod(context.Request.Method);
  requestMessage.RequestUri = new Uri(backendUrl);

  // Copy headers from original request to the new request
  foreach (var header in context.Request.Headers)
  {
   // Skip headers that should not be forwarded
   if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
   {
    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
   }
  }

  // Copy content from original request to the new request
  if (context.Request.ContentLength > 0)
  {
   var contentStream = new MemoryStream();
   await context.Request.Body.CopyToAsync(contentStream);
   contentStream.Seek(0, SeekOrigin.Begin);
   requestMessage.Content = new StreamContent(contentStream);
  }

  // Send the request to the backend
  using var httpClient = new HttpClient();
  var responseMessage = await httpClient.SendAsync(requestMessage);

  // Copy the response from the backend to the original response
  context.Response.StatusCode = (int)responseMessage.StatusCode;

  foreach (var header in responseMessage.Headers)
  {
   context.Response.Headers[header.Key] = header.Value.ToArray();
  }

  foreach (var header in responseMessage.Content.Headers)
  {
   context.Response.Headers[header.Key] = header.Value.ToArray();
  }

  // Ensure CORS headers are not duplicated
  context.Response.Headers.Remove("transfer-encoding");

  // Send the content from the backend response to the original response
  await responseMessage.Content.CopyToAsync(context.Response.Body);
 });

});


// app.UseEndpoints(endpoints => {
//  endpoints.MapControllerRoute(
//   name: "areas",
//   pattern: "{area:exists}/{controller=Home}/{action=Index}");
//
//  //Admin/Home/Index
//  //Admin
//
//  endpoints.MapControllerRoute(
//   name: "default",
//   pattern: "{controller}/{action}/{id?}"
//   );
// }
 //);

//Eg: /persons/edit/1


app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically

