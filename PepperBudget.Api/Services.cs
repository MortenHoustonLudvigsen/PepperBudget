namespace PepperBudget.Api;

public static class Services
{
    public static IHostApplicationBuilder AddPepperBudgetApi(this IHostApplicationBuilder builder, string? connectionString)
    {
        builder.Services.AddControllers();
        builder.Services.AddPepperBudgetData(connectionString);
        return builder;
    }

    public static void UsePepperBudgetApi(this WebApplication app)
    {
        app.MapControllers();
    }
}
