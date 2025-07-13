namespace PepperBudget.Data;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; } // Positive: inflow to Account; Negative: outflow
    public int AccountId { get; set; } // Must be one of your internal accounts
    public Account? Account { get; set; }
    public int CounterAccountId { get; set; } // Can be internal or external
    public Account? CounterAccount { get; set; }
}