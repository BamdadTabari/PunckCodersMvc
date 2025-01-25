using DataProvider.Certain.Configs;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Repository;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using PunckCodersMvc.Configs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Determine connection string based on environment
string connectionString;
if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("ServerDbConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("ProductionDbConnection");
}

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString)
           .EnableDetailedErrors();
});

// Add repositories and services
builder.Services.AddScoped<SecurityTokenConfig>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TokenCleanupTask>();
builder.Services.AddScoped<JobScheduler>();

// Add Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Month)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

// Build the application
var app = builder.Build();

// Configure job scheduler after building the app
using (var scope = app.Services.CreateScope())
{
    var jobScheduler = scope.ServiceProvider.GetRequiredService<JobScheduler>();
    jobScheduler.ConfigureJobs();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Configure middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
