using Learn_web.DataBase;
using Learn_web.Interfaces;
using Learn_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Repository
{
    public class UsersRepository : IUsers
    {
        private OrdersContext context;

        public UsersRepository(OrdersContext context)
        {
            this.context = context;
        }

        public void CreateUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            User findUser = GetUser(id);
            context.Users.Remove(findUser);
            context.SaveChanges();
        }

        public IEnumerable<User> get()
        {
            return context.Users;
        }

        public User GetUser(int id)
        {
            return context.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            context.Attach(user).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
