using FlightDataProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDataProvider.Data
{
    public class FlightDataContext(DbContextOptions<FlightDataContext> options) : DbContext(options)
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Flight> Flights { get; set; }
    }
}
