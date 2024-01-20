using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? GetById(int id);
        bool Create(Car car);
        bool Update(Car car);
        bool Delete(int id);
        bool PlateExists(string plate);
        bool CertificateNrExists(int certificateNr);
    }
}
