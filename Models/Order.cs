using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class Order
    {
        public bool IsComplete;

        ///ID для базы данных
        public int id { get; set; }

        //Дата заказа
        [DataType(DataType.Date)]
        [Display(Name = "Дата заказа")]
        public DateTime dateOrder { get; set; }

        //Данные клиента
        [Display(Name = "Данные клиента")]
        public string clientData { get; set; }

        //С какого языка перевод
        [Display(Name = "Первоначальный язык документа")]
        public string originalLanguage { get; set; }

        //На какой язык передвод
        [Display(Name = "Итоговый язык документа")]
        public string translateLanguage { get; set; }

        //Стоимость работы
        [Column(TypeName = "double(18, 2)")]
        [Display(Name = "Стоимость работы")]
        public double costOfWork { get; set; }

        //Стоимость услуг переводчика
        [Column(TypeName = "double(18, 2)")]
        [Display(Name = "Стоимость услуг переводчика")]
        public double? costOfTranslationServices { get; set; }

        //Переводчик
        [Display(Name = "Переводчик")]
        public string Translator { get; set; }


    }
}
