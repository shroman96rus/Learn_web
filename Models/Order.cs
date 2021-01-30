using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class Order
    {
        ///ID для базы данных
        public int id { get; set; }

        //Дата заказа
        [Display(Name = "Дата заказа")]
        public DateTime dateOrder { get; set; }

        //Данные клиента
        [Display(Name = "Данные клиента")]
        public string clientData { get; set; }

        //С какого языка перевод
        [Display(Name = "С какого языка перевод")]
        public string originalLanguage { get; set; }

        //На какой язык передвод
        [Display(Name = "На какой язык передвод")]
        public string translateLanguage { get; set; }

        //Стоимость работы
        [Display(Name = "Стоимость работы")]
        public double costOfWork { get; set; }

        //Стоимость услуг переводчика
        [Display(Name = "Стоимость услуг переводчика")]
        public double? costOfTranslationServices { get; set; }

        //Переводчик
        [Display(Name = "Переводчик")]
        public string Translator { get; set; }
    }
}
