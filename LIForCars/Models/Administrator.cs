namespace LIForCars.Models
{
    public class Administrator : User
    {
        public int ContractNr { get; set; }

        public Administrator()
            : base()
        {}

        public Administrator(string Name, int Nif, int Cc, string Address, int Phone, char Gender, DateTime BirthDate, string Username, string Email, string Password, bool IsAuctioneer)
            : base(Name, Nif, Cc, Address, Phone, Gender, BirthDate, Username, Email, Password, IsAuctioneer)
        {}

        public Administrator(string Name, int Nif, int Cc, string Address, int Phone, char Gender, DateTime BirthDate, string Username, string Email, string Password, bool IsAuctioneer, int contractNr)
            : base(Name, Nif, Cc, Address, Phone, Gender, BirthDate, Username, Email, Password, IsAuctioneer)
        {
            ContractNr = contractNr;
        }
    }
}