using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassengerService.Web.DbContext;

namespace PassengerService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private readonly UserDbContext _context;

    public TicketController(UserDbContext context, ILogger<TicketController> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    [Route("GetAll")]
    public ActionResult<IList<Ticket>> GetAll()
    {
        return _context.Tickets.ToList();
    }
    
    [HttpGet]
    [Route("GetByUserId")]
    public ActionResult<IList<Ticket>> GetByUserId([FromQuery]string userId)
    {
        return _context.Tickets.Where(c => c.UserId.Equals(userId)).ToList();
    }
    
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<int>> AddTicket([FromBody]Ticket ticket)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        if (ticket.PassengerId != -1 &&
            _context.Passengers.SingleOrDefault(p => p.Id == ticket.PassengerId) == null)
        {
            return NotFound($"Passenger with id: {ticket.Id} not found.");
        }
        await _context.AddAsync(ticket);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok(ticket.Id);
    }
    
    [HttpDelete]
    [Route("RemoveById")]
    public async Task<ActionResult> RemoveById([FromQuery]int id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Tickets.RemoveRange(_context.Tickets.Where(u => u.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateTicketById")]
    public async Task<ActionResult> UpdateTicketById([FromQuery]int id, [FromBody]Ticket ticket)
    {
        var oldTicket = _context.Tickets.FirstOrDefault(u => u.Id == id);
        if (oldTicket == null)
        {
            return NotFound($"Ticket with id: {ticket.Id} not found.");
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Remove(oldTicket);
        await _context.AddAsync(ticket);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}