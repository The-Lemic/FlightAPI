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
    public class LocationsController(FlightDataContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAll()
        {
            return await context.Locations.ToListAsync();
        }

        // Get By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetByID(int id)
        {
            var location = await context.Locations.FindAsync(id);
            if (location == null) 
                return NotFound();
            return location;
        }

        // Create
        [HttpPost]
        public async Task<ActionResult<Location>> Create(CreateLocationDTO dto)
        {
            // Using the DTO to make the object, so ID is concealed
            var location = new Location
            {
                IataCode = dto.IataCode,
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
            };

            context.Locations.Add(location);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByID), new { id = location.Id }, location);
        }

        // Update
        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(int id, UpdateLocationDTO dto)
        {
            // If they have sent the ID in both the URL and the body and they don't match
            var location = await context.Locations.FindAsync(id);

            if (location == null)
                return NotFound();

            location.IataCode = dto.IataCode;
            location.City = dto.City;
            location.Country = dto.Country;
            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var location = await context.Locations.FindAsync(id);

            if (location == null)
                return NotFound();

            context.Locations.Remove(location);

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
