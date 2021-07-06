using Learn_web.Interfaces;
using Learn_web.Models;
using Learn_web.Models.Grafic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Controllers
{
    [Authorize(Policy = "User")]
    public class StatisticController : Controller
    {
        private readonly IOrders Orders;

        public StatisticController(IOrders orders)
        {
            this.Orders = orders;
        }

        

        public IActionResult Index()
        {
            CultureInfo culture = new CultureInfo("ru-Ru", false);
            ViewBag.sum = Convert.ToDecimal(Orders.get().Sum(i => i.costOfWork) - Orders.get().Sum(i => i.costOfTranslationServices)).ToString("C", culture);
            ViewBag.count = Orders.get().Select(i => i.id).Count();

            //ViewBag.sumDay = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute) == DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)).Sum(i => i.costOfWork) - 
            //    Orders.get().Where(i => i.dateOrder.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute) == DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)).Sum(i => i.costOfTranslationServices)).ToString("C", culture);

            ViewBag.sumDay = Orders.get().Where(i => i.dateOrder.ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy")).Sum(i => i.costOfWork)
                - Orders.get().Where(i => i.dateOrder.ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy")).Sum(i => i.costOfTranslationServices);

            ViewBag.countDay = Orders.get().Where(i => i.dateOrder.ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy")).Count();

            DateTime firstDayMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute);

           

            DateTime firstDayWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute);
            ViewBag.sumWeek = Convert.ToDecimal(Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now).Sum(i => i.costOfWork) - Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now)
                .Sum(i => i.costOfTranslationServices)).ToString("C", culture);
            ViewBag.countWeek = Orders.get().Where(i => i.dateOrder >= firstDayWeek && i.dateOrder <= DateTime.Now).Count();

            var test = Orders.get().Where(i => i.dateOrder.Month.ToString() == "1").ToList();
           
            

            return View();
        }

        public JsonResult Month(int month)
        {

            CultureInfo culture = new CultureInfo("ru-Ru", false);
            var sumMonth = Orders.get().Where(i => i.dateOrder.Month == month).Select(i => i.costOfWork).Sum().ToString("C", culture);

            return Json(sumMonth);
        }

        public JsonResult GrafMonth(int month)
        {
            //var countMonth = Orders.get().Where(i => i.dateOrder.Month == month).Select( i => i.costOfWork);

            var allOrder = Orders.get().Where(i => i.dateOrder.Month == month).ToArray();

            List<GraficViewModel> monthSort = new List<GraficViewModel> { new GraficViewModel 
            { 
                summWork = allOrder[0].costOfWork, 
                summDate = allOrder[0].dateOrder.ToString("dd.MM.yyyy")}  
            };

            for (int i = 1, j = 0; i < allOrder.Length; i++, j++)
            {
                if (monthSort[j].summDate == allOrder[i].dateOrder.ToString("dd.MM.yyyy"))
                {
                    monthSort[j].summWork = (monthSort[j].summWork + allOrder[i].costOfWork); j--;
                }
                else
                {
                    monthSort.Add(new GraficViewModel
                    {
                        summWork = allOrder[i].costOfWork,
                        summDate = allOrder[i].dateOrder.ToString("dd.MM.yyyy")
                    });
                }
            }

            //var countMonth = from item in Orders.get() where item.dateOrder.Month == month select new { item.dateOrder, item.costOfWork };

            var countMonth = from item in monthSort select new { item.summDate, item.summWork };


            ViewBag.testmonth = "Январь";

            return Json(countMonth);
        }
    }
}
