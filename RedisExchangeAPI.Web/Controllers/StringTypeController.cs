using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
           
            _redisService =redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {

          
            db.StringSet("name", "Tolga YILDIZ");
            db.StringSet("ziyaretci", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");

            var value2 = db.StringGetRange("name", 0, 3);
            var valueLength = db.StringLength("name");
      
            db.StringIncrement("ziyaretci");
            // var count = db.StringDecrementAsync("ziyaretci", 1).Result;
            var ziyaretci = db.StringGet("ziyaretci");
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
                ViewBag.ziyaretci = ziyaretci;
                ViewBag.value2 = value2;
                ViewBag.length = valueLength;
            }

            return View();
        }
    }
}
