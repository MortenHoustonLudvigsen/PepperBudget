var builder = WebApplication.CreateBuilder(args);

builder.AddPepperBudgetApi(builder.Configuration.GetConnectionString("Default"));

var app = builder.Build();

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

app.UsePepperBudgetApi();

app.MapFallbackToFile("index.html");

app.Run();
