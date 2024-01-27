using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IBidRepository
    {
        IEnumerable<Bid> GetAll();
        Bid? GetById(int id);
        bool Create(Bid bid);
        bool Update(Bid bid);
        bool Delete(int id);
        Task<(int totalBids, IEnumerable<Bid> auctions)> GetBidsAuctionAsync(int idAuction);
        Task<IEnumerable<Bid>> GetBidsUserParticipatedAsync(int userId);
        Task<float?> GetHighestBidAuctionAsync(int auctionId);
    }
}