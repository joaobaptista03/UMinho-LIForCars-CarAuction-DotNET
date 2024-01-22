using LIForCars.Models;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data
{
    public class MyLIForCarsDBContext : DbContext
    {
        public MyLIForCarsDBContext(DbContextOptions<MyLIForCarsDBContext> opt) : base(opt) {}

        public DbSet<User> User { get; set; }
        public DbSet<Auction> Auction { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Bid> Bid { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>(entity =>
            {
                entity.Property(e => e.BasePrice).HasPrecision(18, 2);
                entity.Property(e => e.MinIncrement).HasPrecision(18, 2);
                entity.Property(e => e.BuyNowPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Car)
                .WithOne()
                .HasForeignKey<Auction>(a => a.CarId)
                .OnDelete(DeleteBehavior.SetNull);

            /*    
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Administrator)
                .WithOne()
                .HasForeignKey<Auction>(a => a.AdministratorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Auction>(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            */

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.Property(e => e.BidValue).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany()
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}