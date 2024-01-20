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

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Auction)
                .WithOne(a => a.Car)
                .HasForeignKey<Auction>(a => a.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Car)
                .WithOne(c => c.Auction)
                .HasForeignKey<Car>(c => c.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Administrator>()
                .HasMany(a => a.Auctions)
                .WithOne(a => a.Administrator)
                .HasForeignKey(a => a.AdministratorId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Car> Car { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
    }
}