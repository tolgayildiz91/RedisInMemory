using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2);


            //_distributedCache.SetString("name", "tolga", cacheEntryOptions);
            //await _distributedCache.SetStringAsync("surname", "yildiz", cacheEntryOptions);

            Product product = new Product { ID = 1, Name = "Kalem", Price = 100 };
            Product product2 = new Product { ID = 2, Name = "Kalem2", Price = 100 };
            

            string jsonProduct = JsonConvert.SerializeObject(product);
            string jsonProduct2 = JsonConvert.SerializeObject(product2);

            await _distributedCache.SetStringAsync("product:1",jsonProduct,cacheEntryOptions);
            await _distributedCache.SetStringAsync("product:2", jsonProduct2, cacheEntryOptions);

            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:3",byteproduct);



            return View();
        }

        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");
            //string surname = _distributedCache.GetString("surname");
            //ViewBag.name=name;
            //ViewBag.surname = surname;

            Byte[] byteProduct = _distributedCache.Get("product:3");

            string jsonProduct = _distributedCache.GetString("product:1");

            string jsonProduct3 = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            Product p2 = JsonConvert.DeserializeObject<Product>(jsonProduct3);

            ViewBag.product = p;
            ViewBag.product3 = p2;

            return View();
        }

        public IActionResult Remove()
        {
             _distributedCache.Remove("name");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/ss.png");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim",imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("resim");

            return File(imageByte,"image/png");
        }
    }
}
