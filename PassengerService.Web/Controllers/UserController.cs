using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassengerService.Web.DbContext;

namespace PassengerService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserDbContext _context;

    public UserController(UserDbContext context, ILogger<UserController> logger)
    {
        _logger = logger;
        _context = context;
    }
 
    [HttpGet]
    public IEnumerable<object> List()
    {
        return _context.Users.ToList();
    }
    
    [HttpGet]
    [Route("GetByEmail")]
    public ActionResult<User> GetByEmail(string email)
    {
        var user = _context.Users
            .FirstOrDefault(user => user.Email == email);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
    
    [HttpPost]
    [Route("AddUser")]
    public async Task AddUser([FromBody]User user)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
    }
    
    [HttpDelete]
    [Route("RemoveById")]
    public async Task<ActionResult> RemoveById([FromQuery]int id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        _context.Users.RemoveRange(_context.Users.Where(u => u.Id == id));
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("UpdateUserById")]
    public async Task<ActionResult> UpdateUserById([FromQuery]int id, [FromBody]User user)
    {
        var oldUser = _context.Users.SingleOrDefault(u => u.Id == id);
        if (oldUser == null)
        {
            return NotFound();
        }
        
        await using var tx = await _context.Database.BeginTransactionAsync(); 
        oldUser.Email = user.Email;
        oldUser.Login = user.Login;
        oldUser.Phone = user.Phone;
        oldUser.CompanyId = user.CompanyId;
        oldUser.CompanyName = user.CompanyName;
        oldUser.FirstName = user.FirstName;
        oldUser.MiddleName = user.MiddleName;
        oldUser.ModeratorToken = user.ModeratorToken;
        oldUser.LastName = user.LastName;
        oldUser.ImageUrl = user.ImageUrl;
        oldUser.UserType = user.UserType;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
}