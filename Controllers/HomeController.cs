using Microsoft.AspNetCore.Mvc;

namespace ScamShieldAI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
