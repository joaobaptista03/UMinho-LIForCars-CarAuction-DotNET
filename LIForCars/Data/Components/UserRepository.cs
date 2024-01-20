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

        public User? GetById(int id) => _context.User.FirstOrDefault(c => c.Id == id);
        public User? GetByUsername(string username) => _context.User.FirstOrDefault(c => c.Username == username);
        public bool NifExists(int nif) => _context.User.Any(u => u.Nif == nif);
        public bool CcExists(int cc) => _context.User.Any(u => u.CC == cc);
        public bool PhoneExists(int phone) => _context.User.Any(u => u.Phone == phone);
        public bool UsernameExists(string username) => _context.User.Any(u => u.Username == username);
        public bool EmailExists(string email) => _context.User.Any(u => u.Email == email);

        public bool CheckPassword(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user != null) 
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            return false;
        }


        public bool Create(User newUser)
        {
            try
            {
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password, BCrypt.Net.BCrypt.GenerateSalt());
                _context.User.Add(newUser);
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Update(User newUser)
        {
            try
            {
                _context.User.Update(newUser);
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
            } catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

}