using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;


namespace LIForCars.Data.Components
{
    public class CoworkerRepository : ICoworkerRepository
    {
        private readonly MyLIForCarsDBContext _context;
        private IEnumerable<Coworker> _coworkers = new List<Coworker>();

        public CoworkerRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public bool SaveChanges() => (_context.SaveChanges() >= 0);

        public IEnumerable<Coworker> GetAll() => _context.Coworker.ToList();

        public Coworker GetById(int id) => _context.Coworker.FirstOrDefault(c => c.Id == id);

        public bool Create(Coworker newcoworker)
        {
            try
            {
                _context.Coworker.Add(newcoworker);
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Update(Coworker newcoworker)
        {
            try
            {
                _context.Coworker.Update(newcoworker);
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(Coworker newcoworker)
        {
            try
            {
                _context.Coworker.Remove(newcoworker);
            } catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

}

