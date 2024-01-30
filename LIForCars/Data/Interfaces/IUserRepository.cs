using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IUserRepository
    {
        bool SaveChanges();
        IEnumerable<User> GetAll();
        User? GetByUsername(string username);
        int? GetIdByUsername(string username);
        bool NifExists(int nif);
        bool CcExists(int cc);
        bool PhoneExists(int phone);
        bool UsernameExists(string username);
        bool EmailExists(string email);
        bool CheckPasswordAsync(string username, string password);
        Task<bool> CreateAsync(User newUser);
        bool Update(User newUser);
        bool Delete(User newUser);

        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> IsAdminAsync(string username);
    }
}