using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassengerService.Web.DbContext;

namespace PassengerService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class PassengerController : ControllerBase
{
    private readonly ILogger<WalletController> _logger;
    private readonly UserDbContext _context;

    public PassengerController(UserDbContext context, ILogger<WalletController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("GetPassengers")]
    public ActionResult<List<Passenger>> GetPassengersByUserId([FromQuery] string userId)
    {
        return _context.Passengers.Where(c => c.UserId == userId).ToList();
    }

    [HttpPost]
    [Route("AddPassenger")]
    public async Task<ActionResult<int>> AddPassenger([FromBody] Passenger passenger)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        if (_context.Users.FirstOrDefault(u => u.Id == passenger.UserId) == null)
        {
            return NotFound($"User with id: {passenger.UserId} not found.");
        }
        await _context.AddAsync(passenger);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok(passenger.Id);
    }
    
    [HttpDelete]
    [Route("RemovePassenger")]
    public async Task<ActionResult> RemovePassenger([FromQuery] int id, [FromBody] Passenger passenger)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Passengers.RemoveRange(_context.Passengers.Where(p => p.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdatePassengerById")]
    public async Task<ActionResult> UpdatePassengerById([FromQuery]int id, [FromBody]Passenger passenger)
    {
        var oldPassenger = _context.Passengers.FirstOrDefault(u => u.Id == id);
        if (oldPassenger == null)
        {
            return NotFound($"Passenger with id: {id} not found.");
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        oldPassenger.Document = passenger.Document;
        oldPassenger.Email = passenger.Email;
        oldPassenger.Name = passenger.Name;
        oldPassenger.Phone = passenger.Phone;
        oldPassenger.UserId = passenger.UserId;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}