using Microsoft.EntityFrameworkCore;

namespace PassengerService.Web.DbContext;

public sealed class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<Passenger> Passengers { get; set; }
    
    public DbSet<Ticket> Tickets { get; set; }
    
    public DbSet<Wallet> Wallets { get; set; }
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
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
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

public class Passenger
{
    public int Id { get; set; }
    public string UserId { get; set; }

    public string Name { get; set; }
    
    public string Phone { get; set; }

    public string Email { get; set; }

    public string Document { get; set; }
}

public class Wallet
{
    public string Id { get; set; }
    
    public long Money { get; set; }
}

public class Ticket
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    public int PassengerId { get; set; }
    
    public string Seat { get; set; }
    
    public string Price { get; set; }
    
    public string Status { get; set; }
    
    public int RouteId { get; set; }
}