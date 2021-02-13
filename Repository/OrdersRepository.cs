using Learn_web.DataBase;
using Learn_web.Interfaces;
using Learn_web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

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

        public void deleteOrder(int ID)
        {
            Order findOder = getOrder(ID);
            context.Orders.Remove(findOder);
            context.SaveChanges();
            //return findOder;
        }

        
        public IEnumerable<Order> get()
        {
            return context.Orders;
        }

        public Order getOrder(int id)
        {
           
            return context.Orders.Find(id);
        }

        public void UpdateOrder(Order updateOrder)
        {
            
            context.Attach(updateOrder).State = EntityState.Modified;
            context.SaveChanges();
            
        }
    }
}
