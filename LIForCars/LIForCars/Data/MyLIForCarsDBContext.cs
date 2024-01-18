using LIForCars.Models;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data
{
    public class MyLIForCarsDBContext : DbContext
    {
        public MyLIForCarsDBContext(DbContextOptions<MyLIForCarsDBContext> opt) : base(opt)
        {

        }

        public DbSet<Coworker> Coworker { get; set; }
    }
}