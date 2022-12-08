using Microsoft.AspNetCore.Mvc;

namespace Lacromis.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
