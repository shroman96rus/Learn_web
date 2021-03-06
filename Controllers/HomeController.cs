﻿using Learn_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Learn_web.Interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Learn_web.Controllers
{
    [Authorize(Policy = "User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrders Orders;
        readonly IUsers Users;
        readonly IWebHostEnvironment _appEnvironment;
       

        //Контроллер принимающий данные из контекста
        public HomeController(ILogger<HomeController> logger, IOrders orders, IUsers users, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;

            this.Orders = orders;
            this.Users = users;

            _appEnvironment = appEnvironment;
        }

        //отображение начальной страницы
        
        public IActionResult Index(string search)
        {
            
            var model = from m in Orders.get() select m;
           
            if (!String.IsNullOrEmpty(search))
            {
                if (search == "orderBy")
                {
                    //count++;
                  model = model.OrderBy(i => i.clientData);
                    
                }
                else
                {
                    model = model.Where(i => i.clientData.Contains(search));
                }
                
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
        public async Task<IActionResult> Create(Order order, IFormFile uploadedFile)
        {
            //проверка валидности объекта класса Order
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/files/" + uploadedFile.FileName;
                    using (FileStream fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    order.path = path;
                }
                

                Orders.CreateOrder(order);

                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        //Первичное отображение представления Update
        public IActionResult Update(int id)
        {
            Order order = Orders.getOrder(id);
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

        public IActionResult Detail(int id)
        {
            if (ModelState.IsValid)
            {
                Order detailOrder = Orders.getOrder(id);

                return View(detailOrder);
                
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //Метод отвечающий за удаление объекта из БД на страницу Delete передается данные выбранного обекта
        public IActionResult Delete(int id)
        {
           Order deleteOrder = Orders.getOrder(id);
           
            return View(deleteOrder);
        }

        //метод страницы Delete отвечающий за отображение страницы подтверждения и удаление записи из базы данных
        [HttpPost]
        public IActionResult Delete(Order deleteOrder)
        {
            var test = Orders.getOrder(deleteOrder.id);
            RemoveFileFromServer(test.path);
            Orders.deleteOrder(deleteOrder.id);
            
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        //Загрузка файла
        public async Task<FileResult> Download(string path)
        {
            //var test = Directory.GetCurrentDirectory();
            //Загрузка файла
            var paths = Directory.GetCurrentDirectory() + "\\wwwroot\\" + path;
            //var paths = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files\\", path);
            //var pathss = "C:\\Users\\User\\source\\repos\\Learn_web\\wwwroot\\" + path; //необходимо разобраться почему не работает путь в текущую директорию
            var memory = new MemoryStream();
            using (var stream = new FileStream(paths, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, MediaTypeNames.Application.Octet, Path.GetFileName(paths));
        }

        //Удаление файла
        private bool RemoveFileFromServer(string path)
        {
            string fullPath = _appEnvironment.WebRootPath + path;
            if (!System.IO.File.Exists(fullPath)) return false;
            try
            {
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
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
