using LIForCars.Data.Interfaces;
using LIForCars.Models;

namespace LIForCars.Data.Components
{
    public class CarRepository : ICarRepository
    {
        private readonly MyLIForCarsDBContext _context;

        public CarRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAll()
        {
            return _context.Car.ToList();
        }

        public Car? GetByPlate(string plate)
        {
            return _context.Car.FirstOrDefault(c => c.Plate == plate);
        }

        public bool Create(Car car)
        {
            try {
                _context.Car.Add(car);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public bool Update(Car car)
        {
            try {
                _context.Car.Update(car);
                _context.SaveChanges();
            } catch (Exception) {
                return false;
            }
            return true;

        }

        public bool Delete(string plate)
        {
            try 
            {
                var car = _context.Car.FirstOrDefault(c => c.Plate == plate);
                if (car != null)
                {
                    _context.Car.Remove(car);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool PlateExists(string plate) => _context.Car.Any(c => c.Plate == plate);
        public bool CertificateNrExists(int certificateNr) => _context.Car.Any(c => c.CertificateNr == certificateNr);
    }
}
