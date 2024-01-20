namespace LIForCars.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal BidValue { get; set; }
        public DateTime bidTime { get; set; }
    }
}