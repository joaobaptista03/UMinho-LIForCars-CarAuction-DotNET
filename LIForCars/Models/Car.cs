namespace LIForCars.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int LaunchYear { get; set; }
        public string Plate { get; set; } = string.Empty;
        public int Kms { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public string Motor { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string CertificateNr { get; set; } = string.Empty;
    }
}
