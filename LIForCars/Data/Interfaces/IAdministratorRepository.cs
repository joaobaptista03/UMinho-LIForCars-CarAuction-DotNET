using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IAdministratorRepository
    {
        IEnumerable<Administrator> GetAll();
        Administrator? GetById(int id);
        bool Create(Administrator admin);
        bool Update(Administrator admin);
        bool Delete(int id);
    }
}
