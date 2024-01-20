using LIForCars.Models;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data
{
    public class MyLIForCarsDBContext : DbContext
    {
        public MyLIForCarsDBContext(DbContextOptions<MyLIForCarsDBContext> opt) : base(opt) {}

        public DbSet<User> User { get; set; }
        public DbSet<Auction> Auction { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>(entity =>
            {
                entity.Property(e => e.BasePrice).HasPrecision(18, 2); // Adjust precision and scale as needed
                entity.Property(e => e.MinIncrement).HasPrecision(18, 2);
                entity.Property(e => e.BuyNowPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Car) // Auction has one Car
                .WithMany() // Car does not have a collection of Auctions
                .HasForeignKey(a => a.CarId); // Foreign key in Auction
                .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete
        }

        public DbSet<Car> Car { get; set; }
    }
}