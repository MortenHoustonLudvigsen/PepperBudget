namespace PepperBudget.Data;

public class PepperDbContextFactory : IDesignTimeDbContextFactory<PepperBudgetDbContext>
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=pepperbudget;Username=postgres";
    private static readonly DbContextOptionsBuilder<PepperBudgetDbContext> Builder = new();
    private static readonly DbContextOptions<PepperBudgetDbContext> Options = Builder.UseNpgsql(ConnectionString).Options;
    public PepperBudgetDbContext CreateDbContext(string[] args) => new(Options);
}
