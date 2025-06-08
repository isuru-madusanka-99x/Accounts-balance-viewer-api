using AccountsBalanceViewerAPI.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Auth0.AspNetCore.Authentication;
using System.Security.Claims;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using AccountsBalanceViewerAPI.Infrastructure;
using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Application.Services.Accounts;
using AccountsBalanceViewerAPI.Application.Services.FileUploads;

namespace AccountsBalanceViewerAPI.Startup;

public static partial class ServiceInitializer
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration config, IWebHostEnvironment env)
    {
        services.AddControllers();

        //RegisterScopedServices(services);

        RegisterDbOptions(services, config);

        RegisterServices(services, env);

        RegisterSwagger(services);

        AddAuthentication(services, config, env);

        AddAuthorization(services);

        GenerateLowercaseUrls(services);

        return services;
    }

    private static void RegisterDbOptions(IServiceCollection services, IConfiguration config)
    {
        //configure db repos
        //services.AddTransient<IPlanRepository, PlanRepository>();
        //services.AddTransient<IValveRepository, ValveRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        //Get DB connection string
        IConfiguration connectionConfig = config.GetSection("ConnectionStrings");
        var connectionString = connectionConfig.GetValue<string>("DefaultConnection");

        //Specify migration project for automatic runtime migration
        var migrationsAssemblyName = "AccountsBalanceViewerAPI.Domain";

        //Incorporating retry strategy for database
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString,
            sqlServerOptionsActions =>
            {
                sqlServerOptionsActions.EnableRetryOnFailure(
                    maxRetryCount: 3, // Maximum number of retries
                    maxRetryDelay: TimeSpan.FromSeconds(10), // Maximum delay between retries
                    errorNumbersToAdd: null // Additional error codes to consider transient
                ).MigrationsAssembly(migrationsAssemblyName);
            }
          )
        );
    }

    private static void RegisterSwagger(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        // Configure Swagger with API information
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Accounts Balance Viewer API",
                Version = "v1",
                Description = "API for viewing account balances",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@yourcompany.com"
                }
            });

            // Add JWT Authentication to Swagger
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        //Get configuration section Auth
        IConfiguration _authConfig = config.GetSection("Auth0");

        // Configure Auth0 Authentication
        services.AddAuth0WebAppAuthentication(options => {
            options.Domain = _authConfig.GetValue<string>("Domain") ?? string.Empty;
            options.ClientId = _authConfig.GetValue<string>("ClientId") ?? string.Empty;
            options.ClientSecret = _authConfig.GetValue<string>("ClientSecret");

            options.SkipCookieMiddleware = true;
        });

        // Add JWT Bearer Authentication for API calls
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{_authConfig.GetValue<string>("Domain")}";
                options.Audience = _authConfig.GetValue<string>("Audience");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = "https://schemas.accountsbalanceviewer.com/roles"
                };

                // Add events to handle unauthorized responses appropriately
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        // Skip the default behavior
                        context.HandleResponse();

                        // Return 401 instead of redirect
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            error = "Unauthorized",
                            message = "You are not authorized to access this resource"
                        });

                        return context.Response.WriteAsync(result);
                    }
                };
            });
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        // Add Authorization policies for different user types
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("https://schemas.accountsbalanceviewer.com/roles", "Admin"));

            options.AddPolicy("AllUsers", policy =>
                policy.RequireAuthenticatedUser());
        });
    }

    private static void GenerateLowercaseUrls(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });
    }

    private static void RegisterServices(IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IFileUploadService, FileUploadService>();
    }

    /*

    private static void RegisterScopedServices(IServiceCollection services)
    {
        services.AddScoped<IContextService, ContextService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IAzStorageService, AzStorageService>();
    }

    */
}



