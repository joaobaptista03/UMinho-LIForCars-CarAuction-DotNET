namespace LIForCars.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; }
        public decimal MinIncrement { get; set; }
        public DateTime InitDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal BuyNowPrice { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int AdministratorId { get; set; }
        public Administrator Administrator { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string PicUrl { get; set; } = null!;
    }
}