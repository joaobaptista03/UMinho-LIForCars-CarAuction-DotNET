using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data.Components
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly MyLIForCarsDBContext _context;

        public AuctionRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Auction> GetAll()
        {
            return _context.Auction.ToList();
        }

        public Auction? GetById(int id)
        {
            return _context.Auction.Include(a => a.Car).FirstOrDefault(a => a.Id == id);
        }

        public bool Create(Auction auction)
        {
            try 
            {
                _context.Auction.Add(auction);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool Update(Auction auction)
        {
            try {
                _context.Auction.Update(auction);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try 
            {
                var auction = _context.Auction.Find(id);
                if (auction != null)
                {
                    _context.Auction.Remove(auction);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public bool CarIdExists(int carId) => _context.Auction.Any(a => a.CarId == carId);

        public async Task<(IEnumerable<Auction> auctions, int totalCount)> GetCurrentAuctionsAsync(int page, int pageSize)
        {
            var query = _context.Auction
                .Include(a => a.Car)
                .Where(a => a.InitDateTime <= DateTime.Now && a.EndDateTime >= DateTime.Now);

            var totalCount = await query.CountAsync();
            var auctions = await query.OrderBy(a => a.InitDateTime)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return (auctions, totalCount);
        }
    }
}