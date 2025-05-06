using Microsoft.EntityFrameworkCore;
using coffeeOrder.Models;

namespace coffeeOrder.Data
{
    public class CoffeeOrderDbContext : DbContext
    {
        public CoffeeOrderDbContext(DbContextOptions<CoffeeOrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
