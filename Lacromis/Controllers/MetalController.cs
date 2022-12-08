using Lacromis.DAL;
using Lacromis.Models;
using Lacromis.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lacromis.Controllers
{
    public class MetalController : Controller
    {
        private AppDbContext _context;

        public MetalController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            List<Metal> metal=_context.metals.ToList();
            return View(metal);
        }
        public IActionResult AddBasket(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();

            }
            Metal buyProduct1 = _context.metals.Find(Id);
            if (buyProduct1 == null)
            {
                return NotFound();

            }

            List<BuyProductCount> buyProducts;
            string siu = Request.Cookies["basket"];
            if (siu == null)
            {
                buyProducts = new List<BuyProductCount>();

            }
            else
            {
                buyProducts = JsonConvert.DeserializeObject<List<BuyProductCount>>(Request.Cookies["basket"]);
            }
            BuyProductCount buyProductCount = buyProducts.FirstOrDefault(X => X.Id == buyProduct1.Id);
            if (buyProductCount == null)
            {
                BuyProductCount buyProductCount1 = new BuyProductCount();
                buyProductCount1.Id = buyProduct1.Id;
                buyProductCount1.Name = buyProduct1.Name;
                buyProductCount1.Count = 1;
                buyProducts.Add(buyProductCount1);

            }
            else
            {
                buyProductCount.Count++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(buyProducts), new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Basket()
        {
            List<BuyProductCount> buyProduct = JsonConvert.DeserializeObject<List<BuyProductCount>>(Request.Cookies["basket"]);
            List<BuyProductCount> updatedProducts = new List<BuyProductCount>();

            foreach (var item in buyProduct)
            {
                Metal dbProduct = _context.metals.FirstOrDefault(p => p.Id == item.Id);
                BuyProductCount basketProduct = new BuyProductCount()
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
            List<BuyProductCount> products = JsonConvert.DeserializeObject<List<BuyProductCount>>(Request.Cookies["basket"]);
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

            string basket = Request.Cookies["basket"];

            List<BuyProductCount> products = JsonConvert.DeserializeObject<List<BuyProductCount>>(basket);

            BuyProductCount existProduct = products.FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

            products.Remove(existProduct);

            Response.Cookies.Append(
                "basket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }

        public IActionResult Topla(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["basket"];

            List<BuyProductCount> products = JsonConvert.DeserializeObject<List<BuyProductCount>>(basket);

            BuyProductCount existProduct = products.FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

            Metal dbProdut = _context.metals.FirstOrDefault(p => p.Id == id);

            if (dbProdut.Count > existProduct.Count)
            {
                existProduct.Count++;
            }
            else
            {
                TempData["Fail"] = "not enough count";
                return RedirectToAction("Basket", "Basket");
            }

            Response.Cookies.Append(
                "basket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }

        public IActionResult Cix(int? id)
        {
            if (id == null) return NotFound();

            string basket = Request.Cookies["basket"];

            List<BuyProductCount> products = JsonConvert.DeserializeObject<List<BuyProductCount>>(basket);

            BuyProductCount existProduct = products.FirstOrDefault(p => p.Id == id);

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
                "basket",
                JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) }
                );

            return RedirectToAction(nameof(Basket));
        }

    }


}
