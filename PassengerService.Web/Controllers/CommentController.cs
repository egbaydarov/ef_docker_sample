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
    private readonly ILogger<WalletController> _logger;
    private readonly UserDbContext _context;
    
    public CommentController(UserDbContext context, ILogger<WalletController> logger)
    {
        _logger = logger;
        _context = context;
    }          
    
    [HttpGet]
    [Route("GetAll")]
    public ActionResult<IList<Comment>> GetAll()
    {
        return _context.Comments.ToList();
    }
    
    [HttpGet]
    [Route("GetByUserId")]
    public ActionResult<IList<Comment>> GetByEmail([FromQuery]string userId)
    {
        return _context.Comments.Where(c => c.UserId.Equals(userId)).ToList();
    }
    
    [HttpPost]
    [Route("AddComment")]
    public async Task<ActionResult<int>> AddComment([FromBody]Comment comment)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok(comment.Id);
    }
    
    [HttpDelete]
    [Route("RemoveByUserId")]
    public async Task<ActionResult> RemoveBUserId([FromQuery]string id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Comments.RemoveRange(_context.Comments.Where(u => u.UserId == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateCommentById")]
    public async Task<ActionResult> UpdateCommentByUserId([FromQuery]string userId, [FromBody]Comment comment)
    {
        var oldComment = _context.Comments.FirstOrDefault(u => u.UserId == userId);
        if (oldComment == null)
        {
            return NotFound($"User with id: {userId} not found.");
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Remove(oldComment);
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}