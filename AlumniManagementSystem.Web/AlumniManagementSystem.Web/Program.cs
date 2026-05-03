using AlumniManagementSystem.Web.Components;
using MudBlazor.Services;
using AlumniManagementSystem.Web.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient{
    BaseAddress= new Uri("http://localhost:5016/")
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlumniApiService>();
builder.Services.AddScoped<EventApiService>();
builder.Services.AddScoped<JobApiService>();
builder.Services.AddScoped<DonationApiService>();
builder.Services.AddScoped<MessageApiService>();
builder.Services.AddScoped<DashboardApiService>();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
  app.UseWebAssemblyDebugging();
}
else{
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AlumniManagementSystem.Web.Client._Imports).Assembly);

app.Run();
