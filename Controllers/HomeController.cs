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
using System.Globalization;
using System.Collections.Generic;
using Learn_web.Models.SortPageFilter;
using Learn_web.ViewModels;

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
        
        public IActionResult Index(string search, int page = 1, SortState sortOrder = SortState.dateOrderAsc)
        {
           //получение данных из БД
           var model = Orders.get();
           
            if (!String.IsNullOrEmpty(search))
            {
                  model = model.Where(i => i.clientData.Contains(search));
            }

            //сортировка
            switch (sortOrder)
            {
                case SortState.dateOrderAsc: model = model.OrderBy(i => i.dateOrder);
                    break;
                case SortState.dateOrderDesc:model = model.OrderByDescending(i => i.dateOrder);
                    break;
                case SortState.clientDataAsc: model = model.OrderBy(i => i.clientData);
                    break;
                case SortState.clientDataDesc: model = model.OrderByDescending(i => i.clientData);
                    break;
                case SortState.originalLanguageAsc: model = model.OrderBy(i => i.originalLanguage);
                    break;
                case SortState.originalLanguageDesc: model = model.OrderByDescending(i => i.originalLanguage);
                    break;
                case SortState.translateLanguageAsc: model = model.OrderBy(i => i.translateLanguage);
                    break;
                case SortState.translateLanguageDesc: model = model.OrderByDescending(i => i.translateLanguage);
                    break;
                case SortState.costOfWorkAsc: model = model.OrderBy(i => i.costOfWork);
                    break;
                case SortState.costOfWorkDesc: model = model.OrderByDescending(i => i.costOfWork);
                    break;
                case SortState.costOfTranslationServicesAsc: model = model.OrderBy(i => i.costOfTranslationServices);
                    break;
                case SortState.costOfTranslationServicesDesc: model = model.OrderByDescending(i => i.costOfTranslationServices);
                    break;
                case SortState.TranslatorAsc: model = model.OrderBy(i => i.Translator);
                    break;
                case SortState.TranslatorDesc: model = model.OrderByDescending(i => i.Translator);
                    break;
                default:
                    model = model.OrderBy(i => i.dateOrder);
                    break;
            }

            //Блок отвечающий за пагинацию
            var count = model.Count();

            int pageSize = 20;

            
            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            //создание модели представления
            IndexViewModel viewModel = new IndexViewModel()
            {
                sum = Orders.get().Sum(i => i.costOfWork) - Orders.get().Sum(i => i.costOfTranslationServices),

                pageViewModel = new(count, page, pageSize),

                SortViewModel = new(sortOrder),

                Orders = model


            };

            return View(viewModel);
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
            {   //загрузка файла
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
        public async Task<IActionResult> Update(Order currentOrder, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                //загрузка файла
                if (uploadedFile != null)
                {
                    string path = "/files/" + uploadedFile.FileName;
                    using (FileStream fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    currentOrder.path = path;
                }

                

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
            if (test.path != null)
            {
                RemoveFileFromServer(test.path);
            }
            
            Orders.deleteOrder(deleteOrder.id);
            
            return RedirectToAction("Index");
        }


        public IActionResult Statistic(string month)
        {
            CultureInfo culture = new CultureInfo("ru-Ru", false);
            ViewBag.sum = Convert.ToDecimal(Orders.get().Sum(i => i.costOfWork) - Orders.get().Sum(i => i.costOfTranslationServices)).ToString("C", culture);
            ViewBag.count = Orders.get().Select(i => i.id).Count();
           
            ViewBag.sumDay = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute) == DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)).Sum(i => i.costOfWork) - 
                Orders.get().Where(i => i.dateOrder.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute) == DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)).Sum(i => i.costOfTranslationServices)).ToString("C", culture);
            
            ViewBag.countDay = Orders.get().Where(i => i.dateOrder.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute) == DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)).Count();

            DateTime firstDayMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute);

            if (month != null)
            {
                switch (month)
                {
                    case "Январь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.01.2021") && i.dateOrder <= Convert.ToDateTime("31.01.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.01.2021") && i.dateOrder <= Convert.ToDateTime("31.01.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 1).Count(); break;
                    case "Февраль":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.02.2021") && i.dateOrder <= Convert.ToDateTime("28.02.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.02.2021") && i.dateOrder <= Convert.ToDateTime("28.02.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 2).Count(); break;
                    case "Март":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.03.2021") && i.dateOrder <= Convert.ToDateTime("31.03.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.03.2021") && i.dateOrder <= Convert.ToDateTime("31.03.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 3).Count(); break;
                    case "Апрель":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.04.2021") && i.dateOrder <= Convert.ToDateTime("30.04.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.04.2021") && i.dateOrder <= Convert.ToDateTime("30.04.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 4).Count(); break;
                    case "Май":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.05.2021") && i.dateOrder <= Convert.ToDateTime("31.05.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.05.2021") && i.dateOrder <= Convert.ToDateTime("31.05.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 5).Count(); break;
                    case "Июнь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.06.2021") && i.dateOrder <= Convert.ToDateTime("30.06.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.06.2021") && i.dateOrder <= Convert.ToDateTime("30.06.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 6).Count(); break;
                    case "Июль":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.07.2021") && i.dateOrder <= Convert.ToDateTime("31.07.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.07.2021") && i.dateOrder <= Convert.ToDateTime("31.07.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 7).Count(); break;
                    case "Август":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.08.2021") && i.dateOrder <= Convert.ToDateTime("31.08.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.08.2021") && i.dateOrder <= Convert.ToDateTime("31.08.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 8).Count(); break;
                    case "Сентябрь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.09.2021") && i.dateOrder <= Convert.ToDateTime("30.09.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.09.2021") && i.dateOrder <= Convert.ToDateTime("30.09.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 9).Count(); break;
                    case "Октябрь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.10.2021") && i.dateOrder <= Convert.ToDateTime("31.10.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.10.2021") && i.dateOrder <= Convert.ToDateTime("31.10.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 10).Count(); break;
                    case "Ноябрь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.11.2021") && i.dateOrder <= Convert.ToDateTime("30.11.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.11.2021") && i.dateOrder <= Convert.ToDateTime("30.11.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 11).Count(); break;
                    case "Декабрь":
                        ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.12.2021") && i.dateOrder <= Convert.ToDateTime("31.12.2021"))
                         .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= Convert.ToDateTime("01.12.2021") && i.dateOrder <= Convert.ToDateTime("31.12.2021"))
                         .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                        ViewBag.countMonth = Orders.get().Where(i => i.dateOrder.Month == 12).Count(); break;
                    default: break;
                }
            }
            else {
                ViewBag.sumMonth = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= firstDayMonth && i.dateOrder <= DateTime.Now)
                    .Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= firstDayMonth && i.dateOrder <= DateTime.Now)
                    .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
                ViewBag.countMonth = Orders.get().Where(i => i.dateOrder >= firstDayMonth && i.dateOrder <= DateTime.Now).Count();
            }
            

            DateTime firstDayWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute);
            ViewBag.sumWeek = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now).Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now)
                .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
            ViewBag.countWeek = Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now).Count();

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
        private void RemoveFileFromServer(string path)
        {
            string fullPath = _appEnvironment.WebRootPath + path;
            if (!System.IO.File.Exists(fullPath))
            try
            {
                System.IO.File.Delete(fullPath);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
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
