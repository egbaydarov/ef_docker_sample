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
    public ActionResult<IList<Comment>> GetByUserId([FromQuery]string userId)
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
    [Route("RemoveById")]
    public async Task<ActionResult> RemoveById([FromQuery]int id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Comments.RemoveRange(_context.Comments.Where(u => u.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateCommentById")]
    public async Task<ActionResult> UpdateCommentById([FromQuery]int id, [FromBody]Comment comment)
    {
        var oldComment = _context.Comments.FirstOrDefault(u => u.Id == id);
        if (oldComment == null)
        {
            return NotFound($"Comment with id: {id} not found.");
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync();
        oldComment.Arrival = comment.Arrival;
        oldComment.Departure = comment.Departure;
        oldComment.Email = comment.Email;
        oldComment.Feedback = comment.Feedback;
        oldComment.Phone = comment.Phone;
        oldComment.Rating = comment.Rating;
        oldComment.Vehicle = comment.Vehicle;
        oldComment.CompanyName = comment.CompanyName;
        oldComment.FirstName = comment.FirstName;
        oldComment.LastName = comment.LastName;
        oldComment.MiddleName = comment.MiddleName;
        oldComment.UserId = comment.UserId;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}