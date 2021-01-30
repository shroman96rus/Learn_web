using Learn_web.DataBase;
using Learn_web.Interfaces;
using Learn_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Repository
{
    public class OrdersRepository : IOrders
    {
        private OrdersContext context;

        
        public OrdersRepository(OrdersContext context)
        {
            this.context = context;
        }

       
        public void CreateOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public Order deleteOrder(int ID)
        {
            throw new NotImplementedException();
        }

        
        public IEnumerable<Order> get()
        {
            return context.Orders;
        }

        public Order getOrder(int ID)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
