using Microsoft.EntityFrameworkCore;

namespace PassengerService.Web.DbContext;

public sealed class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
}

public class User
{
    public string Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string Email { get; set; }

    public string Phone { get; set; }
    
    public string Login { get; set; }
    
    public string ModeratorToken { get; set; }
    
    public int CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string UserType { get; set; }

    public string ImageUrl { get; set; }
}

public class Comment
{
    public string Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string Email { get; set; }

    public string Phone { get; set; }
    
    public string CompanyName { get; set; }
    
    public string Vehicle { get; set; }
    
    public string Departure { get; set; }
    
    public string Arrival { get; set; }
    
    public double Rating { get; set; }
    
    public string Feedback { get; set; }
}