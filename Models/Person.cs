﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models
{
    public class Person
    {
        //первичный ключ
        public int Id { get; set; }
        
        //Имя пользователя
        public string nameUser { get; set; }

        //Пароль пользователя
        public string userPassword { get; set; }
    }
}