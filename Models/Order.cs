using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class Order
    {
        ///ID для базы данных
        public int id { get; set; }

        ///Дата заказа
        public DateTime dateOrder { get; set; }

        ///Данные клиента
        public string clientData { get; set; }

        ///С какого языка перевод
        public string originalLanguage { get; set; }

        ///На какой язык передвод
        public string translateLanguage { get; set; }

        ///Стоимость работы
        public double costOfWork { get; set; }

        ///Стоимость услуг переводчика
        public double? costOfTranslationServices { get; set; }

        ///Переводчик
        public string Translator { get; set; }
    }
}
