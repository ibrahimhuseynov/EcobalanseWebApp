using Microsoft.AspNetCore.Mvc;

namespace Lacromis.Areas.Siuuu.Controllers
{
    [Area("Siuuu")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
