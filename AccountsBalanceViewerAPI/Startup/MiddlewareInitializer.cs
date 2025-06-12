namespace AccountsBalanceViewerAPI.Startup;

public static partial class MiddlewareInitializer
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        // Enable Swagger UI for both Development and Production
        // For Azure App Service, we need this enabled
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounts Balance Viewer API v1");
            // Set Swagger UI at the app's root
            c.RoutePrefix = string.Empty;
        });

        // Enable CORS for all origins, methods, and headers
        app.UseCors("AllowDevClient");

        // Add authentication middleware before other middleware
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        

        return app;
    }
}
