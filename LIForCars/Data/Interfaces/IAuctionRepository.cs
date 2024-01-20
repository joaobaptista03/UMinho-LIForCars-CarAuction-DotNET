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
    }
}
