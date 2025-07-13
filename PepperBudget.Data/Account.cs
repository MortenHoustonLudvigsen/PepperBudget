namespace PepperBudget.Data;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsInternal { get; set; } // True for your accounts, false for external (e.g., "Groceries" or "Salary")
    public decimal InitialBalance { get; set; } // Optional starting balance
}
