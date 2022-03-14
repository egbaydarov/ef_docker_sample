using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassengerService.Web.DbContext;

namespace PassengerService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class WalletController : ControllerBase
{
    private readonly ILogger<WalletController> _logger;
    private readonly UserDbContext _context;

    public WalletController(UserDbContext context, ILogger<WalletController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPut]
    [Route("/TopUp")]
    public async Task<ActionResult> TopUp([FromQuery] string userId, long amount)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
        if (wallet == null)
        {
            wallet = new Wallet {UserId = userId};
            _context.Wallets.Add(wallet);
        }
        wallet.Money += amount;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }
    
    [HttpPut]
    [Route("/Withdraw")]
    public async Task<ActionResult> Withdraw([FromQuery] string userId, long amount)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
        if (wallet == null)
        {
            wallet = new Wallet {UserId = userId};
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return NotFound("Not enough money");
        }
        
        wallet.Money -= amount;
        if (wallet.Money < 0)
        {
            return NotFound("Not enough money");
        }
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<Wallet>> GetWallet(string userId)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
        if (wallet != null)
        {
            return wallet;
        }
        
        wallet = new Wallet {UserId = userId};
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return wallet;
    }
}