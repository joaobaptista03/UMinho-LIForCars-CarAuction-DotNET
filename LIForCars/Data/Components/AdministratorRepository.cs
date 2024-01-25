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

        public Administrator? GetByUsername(string username)
        {
            return _context.Administrators.FirstOrDefault(a => a.Username == username);
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

        public bool Delete(string username)
        {
            try {
                var admin = _context.Administrators.FirstOrDefault(a => a.Username == username);
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

        public bool ContractNrExists(int contractNr) => _context.Administrators.Any(a => a.ContractNr == contractNr);
        public bool EmailExists(string email) => _context.Administrators.Any(a => a.Email == email);
        public bool NifExists(int nif) => _context.Administrators.Any(a => a.Nif == nif);
        public bool CCExists(int cc) => _context.Administrators.Any(a => a.CC == cc);
        public bool PhoneExists(int phone) => _context.Administrators.Any(a => a.Phone == phone);
        public bool UsernameExists(string username) => _context.Administrators.Any(a => a.Username == username);

        public bool isAdmin(string username)
        {
            var admin = _context.Administrators.FirstOrDefault(a => a.Username == username);
            return admin != null;
        }
    }
}
