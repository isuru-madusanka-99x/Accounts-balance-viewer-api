namespace AccountsBalanceViewerAPI.Startup;

public static partial class EndpointMapper
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }

}
