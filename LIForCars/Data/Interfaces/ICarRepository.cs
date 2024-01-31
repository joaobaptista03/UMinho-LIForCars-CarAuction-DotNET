using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? GetByPlate(string plate);
        public Car? GetById(int carId);
        bool Create(Car car);
        bool Update(Car car);
        bool Delete(string plate);
        bool PlateExists(string plate);
        bool CertificateNrExists(int certificateNr);
    }
}
