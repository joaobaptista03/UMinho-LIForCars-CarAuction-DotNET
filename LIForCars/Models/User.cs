namespace LIForCars.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Nif { get; set; }
        public int CC { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Phone { get; set; }
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAuctioneer { get; set; } = false;
    }
}