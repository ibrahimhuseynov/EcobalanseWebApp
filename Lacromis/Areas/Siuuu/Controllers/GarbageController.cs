using Lacromis.DAL;
using Lacromis.ExMethods;
using Lacromis.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lacromis.Areas.Siuuu.Controllers
{
    [Area("Siuuu")]
    public class GarbageController : Controller
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;

        public GarbageController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
         List<Garbage> garbage = _context.garbages.ToList();

            return View(garbage);
        }
        public IActionResult Create()
        {
            ViewBag.catagory = new SelectList(_context.catagories.ToList(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Garbage garbage)
        {

            ViewBag.catagory = new SelectList(_context.catagories.ToList(), "Id", "Name");

            if (garbage.Photo != null)
            {
                if (!(garbage.Photo.CheckSize(5)))
                {
                    ModelState.AddModelError("Photo", "5 mb limitinizi kecibsiz");
                    return View();


                }
                if (!(garbage.Photo.CheckType("image/")))
                {
                    ModelState.AddModelError("Photo", "img  formatinda bir seyler at");
                    return View();

                }


                garbage.ImgUrl = await garbage.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "images", "icon"));


            }
            else
            {
                ModelState.AddModelError("Photo", "image elave etmelisen bos kecile bilmez");
                return View();
            }
            garbage.Count = 99;
            _context.garbages.Add(garbage);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int Id)
        {
            ViewBag.catagoryy = _context.catagories.ToList();

            Garbage product = _context.garbages.Find(Id);
            if (product == null)
            {
                return BadRequest();

            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Garbage product, int Id)
        {

            if (Id != product.Id)
            {
                return NotFound();

            }
            Garbage product1 = _context.garbages.Find(Id);
            if (product1 == null)
            {
                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                return View();

            }








            if (product.Photo != null)
            {
                if (!(product.Photo.CheckSize(5)))
                {
                    ModelState.AddModelError("Photo", "5 mb limitinizi kecibsiz");
                    return View();


                }
                if (!(product.Photo.CheckType("image/")))
                {
                    ModelState.AddModelError("Photo", "img  formatinda bir seyler at");
                    return View();

                }

                string suloysu = Path.Combine(_env.WebRootPath, "assets", "images","icon", product1.ImgUrl);
                if (System.IO.File.Exists(suloysu))
                {
                    System.IO.File.Delete(suloysu);

                }

                product1.ImgUrl = await product.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "images","icon"));


            }
            else
            {
                ModelState.AddModelError("Photo", "image elave etmelisen bos kecile bilmez");
                return View();
            }
            product1.Description = product.Description;
            product1.Descrition2 = product.Descrition2;
            product1.Descrition3 = product.Descrition3;
            product1.Price = product.Price;
            product1.Name = product.Name;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
            Garbage product = _context.garbages.Find(Id);
            if (product == null)
            {
                return BadRequest();

            }

            string suloysu = Path.Combine(_env.WebRootPath, "Assets", "images","icon", product.ImgUrl);
            if (System.IO.File.Exists(suloysu))
            {
                System.IO.File.Delete(suloysu);

            }
            _context.garbages.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
