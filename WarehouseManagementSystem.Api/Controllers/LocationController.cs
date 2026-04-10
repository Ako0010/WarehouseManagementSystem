using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly WarehouseManagementDBContext _context;

    public LocationController(WarehouseManagementDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _context.Locations.ToListAsync();
        return Ok(locations);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Create(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return Ok(location);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location is null)
            return NotFound();

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return Ok();
    }
}