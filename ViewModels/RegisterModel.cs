using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [Display(Name = "Имя пользователя")]
        public string nameUser { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string userPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("userPassword", ErrorMessage = "Пароль введен неверно")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Роль в системе")]
        public string role { get; set; }
     
    }
}
