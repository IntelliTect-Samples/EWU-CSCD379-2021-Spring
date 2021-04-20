using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}