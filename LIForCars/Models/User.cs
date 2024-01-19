namespace LIForCars.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Nif { get; set; }
        public int CC { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}