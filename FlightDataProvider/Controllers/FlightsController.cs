using FlightDataProvider.Data;
using FlightDataProvider.DTOs;
using FlightDataProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDataProvider.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController(FlightDataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAll()
        {
            return await context.Flights
                .Include(f => f.Departure)
                .Include(f => f.Arrival)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetByID(int id)
        {
            var flight = await context.Flights
                .Include(f => f.Departure)
                .Include(f => f.Arrival)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
                return NotFound();
            return flight;
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Create(CreateFlightDTO dto)
        {
            var flight = new Flight
            {
                FlightNumber = dto.FlightNumber,
                ArrivalId = dto.ArrivalId,
                ArrivalTime = dto.ArrivalTime,
                DepartureId = dto.DepartureId,
                DepartureTime = dto.DepartureTime,
                Distance = dto.Distance
            };

            await context.Flights.AddAsync(flight);
            await context.SaveChangesAsync();

            // Reload
            var created = await context.Flights
                .Include(f => f.Departure)
                .Include(f => f.Arrival)
                .FirstAsync(f => f.Id == flight.Id);

            return CreatedAtAction(nameof(GetByID), new { id = flight.Id }, created);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(int id, UpdateFlightDTO dto)
        {
            var flight = await context.Flights.FindAsync(id);

            if (flight == null)
                return NotFound();

            flight.FlightNumber = dto.FlightNumber;
            flight.ArrivalId = dto.ArrivalId;
            flight.ArrivalTime = dto.ArrivalTime;
            flight.DepartureId = dto.DepartureId;
            flight.DepartureTime = dto.DepartureTime;
            flight.Distance = dto.Distance;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var flight = await context.Flights.FindAsync(id);

            if (flight == null)
                return NotFound();

            context.Flights.Remove(flight);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
