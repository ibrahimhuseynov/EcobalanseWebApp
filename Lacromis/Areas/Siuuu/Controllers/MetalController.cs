using Lacromis.DAL;
using Lacromis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lacromis.Areas.Siuuu.Controllers
{
    [Area("Siuuu")]
    public class MetalController : Controller
    {
        private AppDbContext _context;

        public MetalController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index(Metal metal)
        {
            List<Metal> metal2 = _context.metals.ToList();
            return View(metal2);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Metal metal)
        {
            metal.Count = 99;
            _context.metals.Add(metal);
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int Id)
        {
          Metal metal=  _context.metals.Find(Id);
            if (metal==null)
            {
                NotFound();

            }

            return View(metal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Metal metal,int Id)
        {
            Metal metal1 = _context.metals.Find(Id);
            if (metal1 == null)
            {
                NotFound();

            }
            else
            {
                metal1.Price = metal.Price;
                metal1.Name = metal.Name;
                metal1.Count = metal.Count;
                _context.SaveChanges();

            }
   
       


            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
            Metal metal = _context.metals.Find(Id);
            if (metal!=null)
            {
                _context.metals.Remove(metal);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
