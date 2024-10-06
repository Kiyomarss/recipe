using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
 [Area("Admin")]
 [Authorize(Roles = "Admin")]
 public class HomeController : Controller
 {
  public IActionResult Index()
  {
   //return View();
   return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "dist", "index.html"), "text/html");

  }
 }
}
