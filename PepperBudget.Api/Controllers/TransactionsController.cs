namespace PepperBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(PepperBudgetDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Transaction>> Get() =>
        await context.Transactions.Include(t => t.Account).Include(t => t.CounterAccount).ToListAsync();

    [HttpGet("{accountId}")]
    public async Task<IEnumerable<Transaction>> GetByAccount(int accountId) =>
        await context.Transactions
            .Where(t => t.AccountId == accountId)
            .Include(t => t.Account).Include(t => t.CounterAccount)
            .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Post(List<Transaction> transactions)
    {
        // Validate: AccountId must be internal
        foreach (var t in transactions)
        {
            var account = await context.Accounts.FindAsync(t.AccountId);
            if (account == null || !account.IsInternal) return BadRequest("Invalid account.");
        }
        context.Transactions.AddRange(transactions);
        await context.SaveChangesAsync();
        return Ok();
    }
}