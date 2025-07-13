namespace PepperBudget.Data;

public class PepperBudgetDbContext(DbContextOptions<PepperBudgetDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasKey(a => a.Id);
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.CounterAccount)
            .WithMany()
            .HasForeignKey(t => t.CounterAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}