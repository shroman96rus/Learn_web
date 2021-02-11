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


        public IActionResult Index(string search)
        {
            
            var model = from m in Orders.get() select m;
           
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(i => i.clientData.Contains(search));
            }
            
            ViewBag.temperature = Weather.GetWeather();


            ViewBag.sum = Orders.get().Sum(i => i.costOfWork) - Orders.get().Sum(i => i.costOfTranslationServices);
            
            return View(model);
        }

        //первичное отображение страницы Create
        public ActionResult Create()
        {
            return View();
        }

        //Передача даных методом post из формы create в БД
        [HttpPost]
        public ActionResult Create(Order order)
        {
            //проверка валидности объекта класса Order
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

        //Первичное отображение представления Update
        public IActionResult Update(int id, Order order)
        {
            order = Orders.getOrder(id);
            return View(order);
        }

        //Передача данных методом post в БД обновленых данных
        [HttpPost]
        public IActionResult Update(Order currentOrder)
        {
            if (ModelState.IsValid)
            {
                Orders.UpdateOrder(currentOrder);

                return RedirectToAction("Index");
            }
            else
            {
                return View(currentOrder);
            }
            
        }

       //Метод отвечающий за удаление объекта из БД на страницу Delete передается данные выбранного обекта
        public IActionResult Delete(int id, Order deleteOrder)
        {
            deleteOrder = Orders.getOrder(id);
           
            return View(deleteOrder);
            
        }

        [HttpPost]
        public IActionResult Delete(Order deleteOrder)
        {
            
            Orders.deleteOrder(deleteOrder.id);
            
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        //Отображение страницы выбранного периода времени
        public IActionResult PeriodSelection(DateTime? firstDate, DateTime? seccondDate)
        {
            var periodOrder = Orders.get();
            
            //Если выбраны обе даты
            if (firstDate != null && seccondDate != null)
            {
                periodOrder = periodOrder.Where(i => i.dateOrder >= firstDate && i.dateOrder <= seccondDate);
            }

            //Если выбрана только дата начала периода
            if (firstDate != null && seccondDate == null)
            {
                periodOrder = periodOrder.Where(i => i.dateOrder >= firstDate);
            }

            //Если выбрана только дата окончания периода
            if (firstDate == null && seccondDate != null)
            {
                periodOrder = periodOrder.Where(i => i.dateOrder <= seccondDate);
            }
            ViewBag.firstDate = firstDate.ToString();
            ViewBag.seccondDate = seccondDate;
            ViewBag.sum = periodOrder.Sum(i => i.costOfWork) - periodOrder.Sum(i => i.costOfTranslationServices);
            return View(periodOrder);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
