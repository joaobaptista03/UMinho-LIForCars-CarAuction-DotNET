using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IUserRepository
    {
        bool SaveChanges();
        IEnumerable<User> GetAll();
        User GetById(int id);
        bool NifExists(int nif);
        bool CcExists(int cc);
        bool PhoneExists(int phone);
        bool UsernameExists(string username);
        bool EmailExists(string email);
        bool Create(User newUser);
        bool Update(User newUser);
        bool Delete(User newUser);
    }
}