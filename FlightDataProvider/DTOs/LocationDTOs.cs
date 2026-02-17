namespace FlightDataProvider.DTOs
{
    public class UpdateLocationDTO
    {
        public required string IataCode { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateLocationDTO
    {
        public required string IataCode { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
