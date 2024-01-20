using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? GetById(int id);
        void Create(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}
