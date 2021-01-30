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

        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
