using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class User
    {
        //первичный ключ
        public int id { get; set; }
        
        //Имя пользователя
        public string nameUser { get; set; }

        //Пароль пользователя
        public string password { get; set; }
    }
}
