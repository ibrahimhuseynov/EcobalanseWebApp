using Microsoft.AspNetCore.Mvc;

namespace Lacromis.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
