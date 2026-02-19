namespace FlightDataProvider.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public required string FlightNumber { get; set; }
        public required int DepartureId { get; set; }
        public Location Departure { get; set; } = null!;
        public required int ArrivalId { get; set; }
        public Location Arrival { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
