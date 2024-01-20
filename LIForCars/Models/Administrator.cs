namespace LIForCars.Models
{
    public class Administrator : User
    {
        public int ContractNumber { get; set; }
        public ICollection<Auction> Auctions { get; set; }
    }
}
