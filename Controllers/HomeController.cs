using Learn_web.Models;
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
        
        public IActionResult Index(string search, int page = 1, SortState sortOrder = SortState.dateOrderDesc)
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

        public ActionResult PartialCreate()
        {

            Order model = new Order();
            return PartialView();
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

        public IActionResult PatritialDetail(int id)
        {
           
                Order detailOrder = Orders.getOrder(id);

                return PartialView(detailOrder);

            
        }

        //Метод отвечающий за удаление объекта из БД на страницу Delete передается данные выбранного обекта
        public IActionResult Delete(int id)
        {
           Order deleteOrder = Orders.getOrder(id);
           
            return View(deleteOrder);
        }

        //Версия метода delete для млдального окна
        public IActionResult PartialDelete(int id)
        {
            Order deleteOrder = Orders.getOrder(id);

            return PartialView(deleteOrder);
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
            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
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
