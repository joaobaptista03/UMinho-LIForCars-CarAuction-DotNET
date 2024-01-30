using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data.Components
{
    public class BidRepository : IBidRepository
    {
        private readonly MyLIForCarsDBContext _context;
        
        public BidRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Bid> GetAll()
        {
            return _context.Bid.ToList();
        }

        public Bid? GetById(int id)
        {
            return _context.Bid.FirstOrDefault(b => b.Id == id);
        }

        public bool Create(Bid bid)
        {
            try {
                _context.Bid.Add(bid);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public bool Update(Bid bid)
        {
            try {
                _context.Bid.Update(bid);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;

        }

        public bool Delete(int id)
        {
            try 
            {
                var bid = _context.Bid.Find(id);
                if (bid != null)
                {
                    _context.Bid.Remove(bid);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<(int totalBids, IEnumerable<Bid> auctions)> GetBidsAuctionAsync(int idAuction) {
            var query = _context.Bid
                            .Include(a => a.User)
                            .Where(a => a.AuctionId == idAuction);
            
            var totalBids = await query.CountAsync();
            var bids = await query.OrderByDescending(a => a.bidTime)
                                  .ToListAsync();
            
            return (totalBids, bids);
        }

        public async Task<IEnumerable<Bid>> GetBidsUserParticipatedAsync(int userId) {

            var auctionsIdsToGet = _context.Bid
                                           .Where(a => a.UserId == userId)
                                           .Select(a => a.AuctionId)
                                           .Distinct()
                                           .ToList();

            var query = _context.Bid
                            .Include(a => a.User)
                            .Include(a => a.Auction)
                            .Include(a => a.Auction.Car)
                            .Where(a => auctionsIdsToGet.Contains(a.AuctionId));
            
            var bids = await query.OrderByDescending(a => a.bidTime)
                                  .ToListAsync();

            return bids;
        }

        public async Task<float?> GetHighestBidAuctionAsync(int auctionId) {

            var highestBidRecord = await _context.Bid
                                           .Where(a => a.AuctionId == auctionId)
                                           .OrderByDescending(a => a.BidValue)
                                           .FirstOrDefaultAsync();

            return highestBidRecord?.BidValue != null ? (float?)highestBidRecord.BidValue : null;
        }

        public async Task<List<IGrouping<Auction, Bid>>?> GetBidsAuctionsUserAsync(IEnumerable<Auction> auctions) {

            var highestBidRecords = await _context.Bid
                                           .Include(a => a.Auction)
                                           .Where(a => auctions.Contains(a.Auction))
                                           .OrderByDescending(a => a.BidValue)
                                           .GroupBy(a => a.Auction)
                                           .ToListAsync();

            return highestBidRecords;
        }
    }
}
