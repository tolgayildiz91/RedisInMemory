using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            ////1. Yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            ////2. Yol
            //if(!_memoryCache.TryGetValue("zaman", out string zamanCache))
            //{
            //    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //    options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            //}


            //2. Yol
            
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //Toplam Ömrü
            options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);

            //Her çağrıldığında uzatılacak süre
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);  

            //Önem değeri
            options.Priority=CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });


            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product p = new Product {ID=1,Name="Kalem",Price=200 };

            _memoryCache.Set<Product>("product:1", p);



            return View();
        }

        public IActionResult Show()
        {

            //_memoryCache.Remove("zaman");

            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{

            //    return DateTime.Now.ToString();
            //});

            // ViewBag.zaman = _memoryCache.Get<string>("zaman");

            _memoryCache.TryGetValue("zaman", out string zamanCache);
            _memoryCache.TryGetValue("callback", out string callback);

            _memoryCache.TryGetValue("product:1", out string product1);

            ViewBag.zaman = zamanCache;
            ViewBag.callback = callback;
            ViewBag.product = product1;

            return View();
        }
    }
}
