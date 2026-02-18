using UtangApp.Api.Services;

namespace UtangApp.Api.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/dashboard");

        group.MapGet("/summary", async (IDashboardService service) =>
        {
            var summary = await service.GetSummaryAsync();
            return Results.Ok(summary);
        });
    }
}
