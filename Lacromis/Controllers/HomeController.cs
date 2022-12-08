using Lacromis.DAL;
using Lacromis.Models;
using Lacromis.ViewModel;
using Lacromis.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lacromis.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            GarbageCatagoryVm garbageCatagoryVm = new GarbageCatagoryVm
            {
                Garbages = _context.garbages.ToList(),
                Catagories = _context.catagories.ToList(),
            };
            List<Garbage> product = _context.garbages.Where(x => x.CatagoryId == 1).ToList();
            List<Garbage> product1 = _context.garbages.Where(x => x.CatagoryId == 2).ToList();
            List<Garbage> product2 = _context.garbages.Where(x => x.CatagoryId == 3).ToList();
            ViewData["product"] = product;
            ViewData["product1"] = product1;
            ViewData["product2"] = product2;

            return View(garbageCatagoryVm);
        }
        public IActionResult AddBasket(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();

            }
            Garbage buyProduct1 = _context.garbages.Find(Id);
            if (buyProduct1 == null)
            {
                return NotFound();

            }

            List<BuyGarbageCount> buyProducts;
            string siu = Request.Cookies["masket"];
            if (siu == null)
            {
                buyProducts = new List<BuyGarbageCount>();

            }
            else
            {
                buyProducts = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(Request.Cookies["masket"]);
            }
            BuyGarbageCount buyProductCount = buyProducts.FirstOrDefault(X => X.Id == buyProduct1.Id);
            if (buyProductCount == null)
            {
                BuyGarbageCount buyProductCount1 = new BuyGarbageCount();
                buyProductCount1.Id = buyProduct1.Id;
                buyProductCount1.Name = buyProduct1.Name;
                buyProductCount1.Description = buyProduct1.Description;
                buyProductCount1.Descriptio2 = buyProduct1.Descrition2;
                buyProductCount1.Descriptio3 = buyProduct1.Descrition3;

                buyProductCount1.Count = 1;
                buyProducts.Add(buyProductCount1);

            }
            else
            {
                buyProductCount.Count++;
            }
            Response.Cookies.Append("masket", JsonConvert.SerializeObject(buyProducts), new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Basket()
        {
            List<BuyGarbageCount> buyProduct = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(Request.Cookies["masket"]);
            List<BuyGarbageCount> updatedProducts = new List<BuyGarbageCount>();

            foreach (var item in buyProduct)
            {
                Garbage dbProduct = _context.garbages.FirstOrDefault(p => p.Id == item.Id);
                BuyGarbageCount basketProduct = new BuyGarbageCount()
                {
                    Id = dbProduct.Id,
                    Price = dbProduct.Price,
                   
                   
                    Name = dbProduct.Name,
                    Count = item.Count

                };

                basketProduct.Price = basketProduct.Price * basketProduct.Count;


                updatedProducts.Add(basketProduct);

            }

            return View(updatedProducts);
        }
        public async Task<IActionResult> InvokeAsync()
        {
            List<BuyGarbageCount> products = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(Request.Cookies["masket"]);
            int cem = 0;
            foreach (var item in products)
            {
                cem += cem;

            }
            ViewBag.kount = cem;

            return View(await Task.FromResult(products));
        }
        public IActionResult RemoveItem(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["masket"];

            List<BuyGarbageCount> products = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(basket);

            BuyGarbageCount existProduct = products.FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

            products.Remove(existProduct);

            Response.Cookies.Append(
                "masket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }

        public IActionResult Topla(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["masket"];

            List<BuyGarbageCount> products = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(basket);

            BuyGarbageCount existProduct = products.FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

                Garbage dbProdut = _context.garbages.FirstOrDefault(p => p.Id == id);

            if (dbProdut.Count > existProduct.Count)
            {
                existProduct.Count++;
            }
            else
            {
                TempData["Fail"] = "not enough count";
                return RedirectToAction("Masket", "Masket");
            }

            Response.Cookies.Append(
                "masket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }

        public IActionResult Cix(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["masket"];

            List<BuyGarbageCount> products = JsonConvert.DeserializeObject<List<BuyGarbageCount>>(basket);

            BuyGarbageCount existProduct = products.FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

            if (existProduct.Count > 1)
            {
                existProduct.Count--;
            }
            else
            {
                RemoveItem(existProduct.Id);

                return RedirectToAction(nameof(Basket));
            }


            Response.Cookies.Append(
                "masket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }
    }
}
