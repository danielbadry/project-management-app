using AppHost.Web;
using AppHost.Web.Components;
using AppHost.Web.Services;
using AppHost.Web.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Ensure server logs appear in the terminal and VS debug output.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<ProjectsService>();
builder.Services.AddScoped<SubTasksService>();
builder.Services.AddScoped<StoriesService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddOutputCache();

builder.Services.AddScoped<
    AuthenticationStateProvider,
    CustomAuthenticationStateProvider>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(
        builder.Environment.IsDevelopment()
            ? "http://apiservice"
            : "https+http://apiservice");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
