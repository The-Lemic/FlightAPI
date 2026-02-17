using FlightDataProvider.Data;
using FlightDataProvider.DTOs;
using FlightDataProvider.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDataProvider.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController(FlightDataContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAll()
        {
            return await context.Flights.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetByID(int id)
        {
            var flight = await context.Flights.FindAsync(id);
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
                Arrival = dto.Arrival,
                ArrivalTime = dto.ArrivalTime,
                Departure = dto.Departure,
                DepartureTime = dto.DepartureTime
            };

            await context.Flights.AddAsync(flight);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByID), new { id = flight.Id }, flight);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(int id, UpdateFlightDTO dto)
        {
            var flight = await context.Flights.FindAsync(id);

            if (flight == null)
                return NotFound();

            flight.FlightNumber = dto.FlightNumber;
            flight.Arrival = dto.Arrival;
            flight.ArrivalTime = dto.ArrivalTime;
            flight.Departure = dto.Departure;
            flight.DepartureTime = dto.DepartureTime;

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
