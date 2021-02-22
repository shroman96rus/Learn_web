using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class User
    {
        //первичный ключ
        public int Id { get; set; }

        //Имя пользователя
        [Display(Name = "Имя пользователя")]
        public string nameUser { get; set; }

        //Пароль пользователя
        [Display(Name = "Пароль")]
        public string userPassword { get; set; }

        [Display(Name = "Роль в системе")]
        public string role { get; set; }
    }
}
