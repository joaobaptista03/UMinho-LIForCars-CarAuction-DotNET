using BCrypt.Net;
using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace LIForCars.Data.Components
{
    public class UserRepository : IUserRepository
    {
        private readonly MyLIForCarsDBContext _context;
        private IEnumerable<User> _Users = new List<User>();

        public UserRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public bool SaveChanges() => (_context.SaveChanges() >= 0);

        public IEnumerable<User> GetAll() => _context.User.ToList();

        public User? GetByUsername(string username) => _context.User.FirstOrDefault(c => c.Username == username);
        public int? GetIdByUsername(string username) => _context.User.FirstOrDefault(c => c.Username == username)?.Id;
        public bool NifExists(int nif) => _context.User.Any(u => u.Nif == nif);
        public bool CcExists(int cc) => _context.User.Any(u => u.CC == cc);
        public bool PhoneExists(int phone) => _context.User.Any(u => u.Phone == phone);
        public bool UsernameExists(string username) => _context.User.Any(u => u.Username == username);
        public bool EmailExists(string email) => _context.User.Any(u => u.Email == email);

        public bool CheckPasswordAsync(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user != null) 
                return BCrypt.Net.BCrypt.Verify(password, user.Password);

            user =  _context.User.FirstOrDefault(u => u.Email == username);
            if (user != null) 
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            return false;
        }

        public bool Update(User newUser)
        {
            try
            {
                _context.User.Update(newUser);
                _context.SaveChanges();
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(User newUser)
        {
            try
            {
                _context.User.Remove(newUser);
                _context.SaveChanges();
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CreateAsync(User newUser)
        {
            try
            {
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> IsAdminAsync(string username)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
                return user is Administrator;
            return false;
        }

    }

}