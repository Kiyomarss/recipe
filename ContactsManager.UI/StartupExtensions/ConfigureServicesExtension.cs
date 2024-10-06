using ContactsManager.Core.Domain.IdentityEntities;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample
{
 public static class ConfigureServicesExtension
 {
  public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
  {
   services.AddTransient<ResponseHeaderActionFilter>();

   //it adds controllers and views as services
   services.AddControllersWithViews(options => {
    //options.Filters.Add<ResponseHeaderActionFilter>(5);

    var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

    options.Filters.Add(new ResponseHeaderActionFilter(logger)
    {
     Key = "My-Key-From-Global",
     Value = "My-Value-From-Global",
     Order = 2
    });

    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());//توکن ضد جعل درخواست
   });


   //add services into IoC container
   services.AddScoped<ICountriesRepository, CountriesRepository>();
   services.AddScoped<IPersonsRepository, PersonsRepository>();
   services.AddScoped<IRecipesRepository, RecipesRepository>();
   services.AddScoped<IIngredientsRepository, IngredientsRepository>();
   
   services.AddScoped<ICountriesGetterService, CountriesGetterService>();
   services.AddScoped<ICountriesAdderService, CountriesAdderService>();
   services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

   services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
   services.AddScoped<PersonsGetterService, PersonsGetterService>();

   services.AddScoped<IRecipesAdderService, RecipesAdderService>();
   services.AddScoped<IRecipesGetterService, RecipesGetterService>();
   
   services.AddScoped<IPersonsAdderService, PersonsAdderService>();
   services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
   services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
   services.AddScoped<IPersonsSorterService, PersonsSorterService>();
   services.AddTransient<PersonsListActionFilter>();

   services.AddDbContext<ApplicationDbContext>(options =>
   {
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
   });


   //Enable Identity in this project
   services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 3; //Eg: AB12AB (unique characters are A,B,1,2)
   })
    .AddEntityFrameworkStores<ApplicationDbContext>()

    .AddDefaultTokenProviders()

    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()

    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
   
   services.AddCors(options =>
   {
    options.AddPolicy("CorsPolicy",
     builder => builder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());
   });

   services.AddControllers();
   
   services.AddControllers().AddJsonOptions(options =>
   {
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
   });
   
   services.AddControllers().AddNewtonsoftJson(options =>
   {
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
     NamingStrategy = new CamelCaseNamingStrategy() // برای اطمینان از تطابق نام‌ها با JSON
    };
   });

   
   services.AddAuthorization(options =>
   {
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); //enforces authoriation policy (user must be authenticated) for all the action methods

    options.AddPolicy("NotAuthorized", policy =>
    {
     policy.RequireAssertion(context =>
     {
      return !context.User.Identity.IsAuthenticated;
     });
    });
   });

   services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
   });

   services.AddHttpLogging(options =>
   {
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
   });

   return services;
  }
 }
}
