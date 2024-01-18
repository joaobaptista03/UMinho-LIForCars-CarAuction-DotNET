using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface ICoworkerRepository
    {
        bool SaveChanges();
        IEnumerable<Coworker> GetAll();
        Coworker GetById(int id);
        bool Create(Coworker newCoworker);
        bool Update(Coworker newCoworker);
        bool Delete(Coworker newCoworker);
    }
}
