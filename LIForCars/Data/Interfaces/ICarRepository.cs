using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? GetByPlate(string plate);
        bool Create(Car car);
        bool Update(Car car);
        bool Delete(string plate);
        bool PlateExists(string plate);
        bool CertificateNrExists(int certificateNr);
    }
}
