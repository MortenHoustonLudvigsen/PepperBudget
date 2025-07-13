namespace PepperBudget.Data;

public static class Services
{
    public static IServiceCollection AddPepperBudgetData(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<PepperBudgetDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
        return services;
    }
}
