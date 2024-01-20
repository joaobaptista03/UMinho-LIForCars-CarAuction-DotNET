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

        public Car? GetById(int id)
        {
            return _context.Car.FirstOrDefault(c => c.Id == id);
        }

        public void Create(Car car)
        {
            _context.Car.Add(car);
            _context.SaveChanges();
        }

        public void Update(Car car)
        {
            _context.Car.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.Car.Find(id);
            if (car != null)
            {
                _context.Car.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}
