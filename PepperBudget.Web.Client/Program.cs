var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<PepperBudget.Web.Client.Services.BudgetService>();
builder.Services.AddScoped<PepperBudget.Web.Client.Services.PageInfoService>();

await builder.Build().RunAsync();
