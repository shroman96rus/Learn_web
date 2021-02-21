using Learn_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Interfaces
{
    public interface IUsers
    {
        IEnumerable<User> get();

        User GetUser(int id);

        void CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
    }
}
