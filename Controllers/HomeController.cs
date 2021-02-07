using Learn_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Learn_web.Repository;
using Learn_web.DataBase;
using Learn_web.Interfaces;
using Learn_web.Models;
using Newtonsoft.Json;

namespace Learn_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        IOrders Orders;

        //Контроллер принимающий данные из контекста
        public HomeController(ILogger<HomeController> logger, IOrders orders)
        {
            _logger = logger;

            this.Orders = orders;
        }

        public IActionResult Index()
        {
           var model = Orders.get();

            ViewBag.temperature = GetWeather();

            var sum = Orders.get().Sum(i => i.costOfWork) - Orders.get().Sum(i => i.costOfTranslationServices);
            ViewBag.sum = sum;

            return View(model);
        }

        
       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {

            if (ModelState.IsValid)
            {
                
                Orders.CreateOrder(order);

                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        [HttpPost]
        public IActionResult Update(Order currentOrder)
        {

            Orders.UpdateOrder(currentOrder);

            return RedirectToAction("Index");
            
        }

        public IActionResult Update(int id, Order order)
        {
            order = Orders.getOrder(id);
            return View(order);
        }

        public IActionResult Delete(int id)
        {
            Orders.deleteOrder(id);
            return RedirectToAction("Index");
        }

       
        public IActionResult Privacy()
        {
            return View();
        }


        public string GetWeather()
        {
            Weather weather = new Weather();

            string respone = weather.TestWeather();

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(respone);

            return $"Погода в {weatherResponse.Name} состовляет {weatherResponse.Main.Temp} градусов цельсия";


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
