
`GrokExport.json`:

```json
{
  "RootDirectory": "D:/Projects/PepperBudget",
  "IncludeAll": false,
  "ChangedFiles": true,
  "IncludedFiles": [
	{
		"Files": [
		    "*.sln",
		    "**/*",
		],
		"OutputFile": "GrokExport/GrokExport.md",
		"Include": true
	},
  ]
}
```

`PepperBudget.Web.Client/Layout/Header.razor`:

```
@implements IDisposable
@inject PepperBudget.Web.Client.Services.PageInfoService PageInfo

<RadzenHeader>
    <RadzenStack Orientation="Orientation.Horizontal" AlignItems="AlignItems.Center" JustifyContent="JustifyContent.SpaceBetween" Gap="0">
        <!-- Left-aligned items -->
        <RadzenStack Orientation="Orientation.Horizontal" AlignItems="AlignItems.Center" Gap="0">
            <RadzenSidebarToggle Click="ToggleMenu" />
            <RadzenLabel Text="@PageInfo.PageTitle" />
        </RadzenStack>

        <!-- Right-aligned icons -->
        <RadzenStack Orientation="Orientation.Horizontal" Gap="1rem">
            <RadzenAppearanceToggle class="rz-mx-2" />
        </RadzenStack>
    </RadzenStack>
</RadzenHeader>

@code {
    [Parameter]
    public EventCallback MenuToggled { get; set; }

    private async Task ToggleMenu()
    {
        await MenuToggled.InvokeAsync();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        PageInfo.OnChanged += StateChanged;
    }

    private void StateChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        PageInfo.OnChanged -= StateChanged;
    }
}
```

`PepperBudget.Web.Client/Pages/Home.razor`:

```
@page "/"
@inject PepperBudget.Web.Client.Services.BudgetService BudgetService

<h1>Pepper Budget</h1>

<!-- Add Account Form -->
<RadzenCard>
    <h3>Add Account</h3>
    <RadzenTextBox @bind-Value="newAccount.Name" Placeholder="Name" />
    <RadzenCheckBox @bind-Value="newAccount.IsInternal" /> Internal (Your Account)
    <RadzenButton Click="AddAccount" Text="Save" />
</RadzenCard>

<!-- Select Account for Import/View -->
<RadzenDropDown @bind-Value="selectedAccountId" Data="@internalAccounts" TextProperty="Name" ValueProperty="Id" Placeholder="Select Account" />

<RadzenUpload ChooseText="Import CSV for Selected Account" Multiple="false" Auto="false" Change="@ImportFile" Disabled="!selectedAccountId.HasValue" />

<RadzenDataGrid @ref="grid" Data="@transactions" TItem="Transaction">
    <Columns>
        <RadzenDataGridColumn Property="Date" Title="Date" />
        <RadzenDataGridColumn Property="Description" Title="Description" />
        <RadzenDataGridColumn Property="Amount" Title="Amount" />
        <RadzenDataGridColumn Property="Account.Name" Title="Account" />
        <RadzenDataGridColumn Property="CounterAccount.Name" Title="Counter Account" />
    </Columns>
</RadzenDataGrid>

<h2>Projection (Next 3 Months Expenses)</h2>
<RadzenDataGrid Data="@projection" TItem="KeyValuePair<string, decimal>">
    <Columns>
        <RadzenDataGridColumn Property="Key" Title="Category (External)" />
        <RadzenDataGridColumn Property="Value" Title="Projected Spend" />
    </Columns>
</RadzenDataGrid>

@code {
    private Account[] internalAccounts = [];
    private int? selectedAccountId;
    private Account newAccount = new();
    private Transaction[] transactions = [];
    private Dictionary<string, decimal> projection = [];
    private RadzenDataGrid<Transaction>? grid;

    protected override async Task OnInitializedAsync()
    {
        internalAccounts = await BudgetService.GetAccountsAsync(true);
        await LoadData();
    }

    private async Task LoadData()
    {
        transactions = await BudgetService.GetTransactionsAsync(selectedAccountId);
        projection = PepperBudget.Web.Client.Services.BudgetService.ProjectBudget(transactions, 3);
        if (grid != null) await grid.Reload();
    }

    private async Task AddAccount()
    {
        await BudgetService.AddAccountAsync(newAccount);
        internalAccounts = await BudgetService.GetAccountsAsync(true);
        newAccount = new();
        StateHasChanged();
    }

    private async Task ImportFile(UploadChangeEventArgs args)
    {
        if (args.Files.Any() && selectedAccountId.HasValue)
        {
            var file = args.Files.First();
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var csv = reader.ReadToEnd();
            await BudgetService.ImportTransactionsAsync(csv);
            await LoadData();
        }
    }
}
```

`PepperBudget.Web.Client/Program.cs`:

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<PepperBudget.Web.Client.Services.BudgetService>();
builder.Services.AddScoped<PepperBudget.Web.Client.Services.PageInfoService>();

await builder.Build().RunAsync();
```

`PepperBudget.Web.Client/Properties/GlobalUsings.cs`:

```csharp
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.Extensions.DependencyInjection;

global using PepperBudget.Data.Models;
//global using PepperBudget.Web.Client.Services;

global using Radzen;

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http;
global using System.Net.Http.Json;
global using System.Threading.Tasks;
```

`PepperBudget.Web.Client/_Imports.razor`:

```
@using System.Globalization
@using System.IO
@using System.Net.Http
@using System.Net.Http.Json

@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop

@using PepperBudget.Web.Client
@* @using PepperBudget.Web.Client.Services; *@

@using Radzen
@using Radzen.Blazor

@using static Microsoft.AspNetCore.Components.Web.RenderMode
```
