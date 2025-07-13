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
    public async Task<IActionResult> Post(string csv)
    {
        throw new NotImplementedException();
    }
}
