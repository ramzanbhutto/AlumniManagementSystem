using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using AlumniManagementSystem.Web.Client.Services;

var builder= WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient{
    BaseAddress = new Uri("http://localhost:5016/")
});

// App services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlumniApiService>();
builder.Services.AddScoped<EventApiService>();
builder.Services.AddScoped<JobApiService>();
builder.Services.AddScoped<DonationApiService>();
builder.Services.AddScoped<MessageApiService>();
builder.Services.AddScoped<DashboardApiService>();

await builder.Build().RunAsync();
