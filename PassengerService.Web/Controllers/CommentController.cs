using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassengerService.Web.DbContext;

namespace PassengerService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserDbContext _context;
    
    public CommentController(UserDbContext context, ILogger<UserController> logger)
    {
        _logger = logger;
        _context = context;
    }          
    
    [HttpGet]
    [Route("GetAll")]
    public ActionResult<IList<Comment>> GetByEmail()
    {
        return _context.UsersComments.ToList();
    }
    
    [HttpPost]
    [Route("AddComment")]
    public async Task AddComment([FromBody]Comment user)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
    }
    
    [HttpPost]
    [Route("RemoveById")]
    public async Task<ActionResult> RemoveById([FromQuery]int id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.UsersComments.RemoveRange(_context.UsersComments.Where(u => u.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateCommentById")]
    public async Task<ActionResult> UpdateUserByEmail([FromQuery]int id, [FromBody]User user)
    {
        var oldUser = _context.UsersComments.FirstOrDefault(u => u.Id == id);
        if (oldUser == null)
        {
            return NotFound();
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Remove(oldUser);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}