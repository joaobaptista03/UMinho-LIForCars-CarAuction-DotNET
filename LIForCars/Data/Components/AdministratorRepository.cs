using LIForCars.Models;
using LIForCars.Data.Interfaces;

namespace LIForCars.Data.Components
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly MyLIForCarsDBContext _context;

        public AdministratorRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Administrator> GetAll()
        {
            return _context.Administrators.ToList();
        }

        public Administrator? GetById(int id)
        {
            return _context.Administrators.FirstOrDefault(a => a.Id == id);
        }

        public bool Create(Administrator admin)
        {
            try {
                _context.Administrators.Add(admin);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public bool Update(Administrator admin)
        {
            try {
                _context.Administrators.Update(admin);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            try {
                var admin = _context.Administrators.Find(id);
                if (admin != null)
                {
                    _context.Administrators.Remove(admin);
                    _context.SaveChanges();
                }
            } catch (Exception) {
                return false;
            }
            return true;
        }
    }
}
