using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {

            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            List<string> names = new();
            if (db.KeyExists(listKey)) 
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    names.Add(x.ToString());
                });
            }
            return View(names);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);


            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListRightPopAsync(listKey).Wait();
            return RedirectToAction("Index");
        }


    }
}
