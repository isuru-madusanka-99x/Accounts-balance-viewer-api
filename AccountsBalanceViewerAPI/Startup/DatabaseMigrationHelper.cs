using AccountsBalanceViewerAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccountsBalanceViewerAPI.Startup;

public static class DatabaseMigrationHelper
{
    public static IHost ExecuteDbMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var _dbMigrationConfig = config.GetSection("DbMigrations");
            if (!_dbMigrationConfig.GetValue<bool>("EnableAutoMigration") && dbContext.Database.GetPendingMigrations().Any())
            {
                // throw an exception when Auto migration is disabled and there are pending migrations
                throw new InvalidOperationException("Auto migration is disabled, but there are pending migrations.");
            }
            else if (_dbMigrationConfig.GetValue<bool>("EnableAutoMigration") && dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
        return app;
    }
}
