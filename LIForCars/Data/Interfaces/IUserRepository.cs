using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IUserRepository
    {
        bool SaveChanges();
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByNif(int nif);
        User GetByCC(int cc);
        User GetByUsername(string username);
        bool Create(User newUser);
        bool Update(User newUser);
        bool Delete(User newUser);
    }
}
