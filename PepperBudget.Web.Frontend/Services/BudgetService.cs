namespace PepperBudget.Web.Frontend.Services;

public class BudgetService(HttpClient http)
{
    public async Task<Account[]> GetAccountsAsync(bool internalOnly = false)
    {
        var accounts = await http.GetFromJsonAsync<Account[]>("api/accounts") ?? [];
        return [.. accounts.Where(a => !internalOnly || a.IsInternal)];
    }

    public async Task AddAccountAsync(Account account) =>
        await http.PostAsJsonAsync("api/accounts", account);

    public async Task<Transaction[]> GetTransactionsAsync(int? accountId = null)
    {
        var url = accountId.HasValue ? $"api/transactions/{accountId}" : "api/transactions";
        return await http.GetFromJsonAsync<Transaction[]>(url) ?? [];
    }

    public async Task ImportTransactionsAsync(string csv) =>
        await http.PostAsJsonAsync("api/transactions", csv);

    public static Dictionary<string, decimal> ProjectBudget(IEnumerable<Transaction> transactions, int monthsAhead)
    {
        // Project outflows (expenses) based on external counter accounts
        var monthlyAverages = transactions
            .Where(t => t.Amount < 0 && !(t.CounterAccount?.IsInternal ?? false))
            .GroupBy(t => t.CounterAccount?.Name ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Average(t => -t.Amount));

        return monthlyAverages.ToDictionary(kvp => kvp.Key, kvp => kvp.Value * monthsAhead);
    }
}
