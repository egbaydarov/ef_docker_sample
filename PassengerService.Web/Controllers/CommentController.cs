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
        return _context.Comments.ToList();
    }
    
    [HttpPost]
    [Route("AddComment")]
    public async Task AddComment([FromBody]Comment comment)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
    }
    
    [HttpDelete]
    [Route("RemoveById")]
    public async Task<ActionResult> RemoveById([FromQuery]string id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Comments.RemoveRange(_context.Comments.Where(u => u.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateCommentById")]
    public async Task<ActionResult> UpdateUserByEmail([FromQuery]string id, [FromBody]Comment comment)
    {
        var oldComment = _context.Comments.FirstOrDefault(u => u.Id == id);
        if (oldComment == null)
        {
            return NotFound();
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Remove(oldComment);
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}