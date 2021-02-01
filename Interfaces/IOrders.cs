using Learn_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Interfaces
{
   public interface IOrders
    {
        IEnumerable<Order> get();

        Order getOrder(int ID);

        void CreateOrder(Order order);

        void UpdateOrder(Order order);

        void deleteOrder(int ID);
    }
}
