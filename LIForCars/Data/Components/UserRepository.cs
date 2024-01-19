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

        public User GetById(int id) => _context.User.FirstOrDefault(c => c.Id == id);

        public bool Create(User newUser)
        {
            try
            {
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
