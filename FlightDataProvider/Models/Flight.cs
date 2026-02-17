namespace FlightDataProvider.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public required string FlightNumber { get; set; }
        public required Location Departure { get; set; }
        public required Location Arrival { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
