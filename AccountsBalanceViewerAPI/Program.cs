using AccountsBalanceViewerAPI.Startup;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;

// Get configuration
IConfiguration config = builder.Configuration;

// Add services to the container.
builder.Services.ConfigureServices(config, env);

var app = builder.Build();

app.RegisterEndpoints();
app.ConfigureMiddleware();

// Migrate latest database changes during startup if auto migration is enabled
app.ExecuteDbMigrations();

app.Run();