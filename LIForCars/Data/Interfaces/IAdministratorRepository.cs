using LIForCars.Models;

namespace LIForCars.Data.Interfaces
{
    public interface IAdministratorRepository
    {
        IEnumerable<Administrator> GetAll();
        Administrator? GetByUsername(string username);
        bool Create(Administrator admin);
        bool Update(Administrator admin);
        bool Delete(string username);
        bool ContractNrExists(int contractNr);
        bool EmailExists(string email);
        bool NifExists(int nif);
        bool CCExists(int cc);
        bool PhoneExists(int phone);
        bool UsernameExists(string username);
        bool IsAdmin(string username);
    }
}
