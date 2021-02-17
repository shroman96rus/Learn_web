using Learn_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Learn_web.DataBase
{
    public class OrdersContext: DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
