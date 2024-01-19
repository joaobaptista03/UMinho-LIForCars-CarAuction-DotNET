using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IUserRepository
    {
        bool SaveChanges();
        IEnumerable<User> GetAll();
        User GetById(int id);
        bool Create(User newUser);
        bool Update(User newUser);
        bool Delete(User newUser);
    }
}
