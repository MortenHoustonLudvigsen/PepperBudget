namespace PepperBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(PepperBudgetDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Account>> Get() => await context.Accounts.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Post(Account account)
    {
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return Ok(account);
    }
}
