using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IAuctionRepository
    {
        IEnumerable<Auction> GetAll();
        Auction? GetById(int id);
        bool Create(Auction auction);
        bool Update(Auction auction);
        bool Delete(int id);
        bool CarIdExists(int carId);

        Task<(IEnumerable<Auction> auctions, int totalCount)> GetCurrentAuctionsAsync(int page, int pageSize, string orderBy);
        Task<(IEnumerable<Auction> auctions, int totalCount)> GetAuctionsUserAsync(int page, int pageSize, int idUser);
        Task<IEnumerable<Auction>> GetAuctionsUserWaitingApprovalAsync(int page, int pageSize, int idUser);

    }
}