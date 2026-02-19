using FlightDataProvider.Models;

namespace FlightDataProvider.DTOs
{
    public class CreateFlightDTO
    {
        public required string FlightNumber { get; set; }
        public required int DepartureId { get; set; }
        public required int ArrivalId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public double Distance { get; set; }
    }
    public class UpdateFlightDTO
    {
        public required string FlightNumber { get; set; }
        public required int DepartureId { get; set; }
        public required int ArrivalId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
