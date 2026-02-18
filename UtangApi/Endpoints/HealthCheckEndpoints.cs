using Microsoft.Extensions.Diagnostics.HealthChecks;
using UtangApp.Api.Data;

namespace UtangApp.Api.Endpoints;

public static class HealthCheckEndpoints
{
    public static void MapHealthCheckEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/health");

        group.MapGet("/", async (HealthCheckService healthCheckService) =>
        {
            var result = await healthCheckService.CheckHealthAsync();
            var response = new
            {
                status = result.Status.ToString().ToLowerInvariant(),
                duration = result.TotalDuration.TotalMilliseconds,
                checks = result.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString().ToLowerInvariant(),
                    duration = e.Value.Duration.TotalMilliseconds,
                    description = e.Value.Description,
                    exception = e.Value.Exception?.Message
                })
            };

            return result.Status == HealthStatus.Healthy
                ? Results.Ok(response)
                : Results.Json(response, statusCode: 503);
        });

        group.MapGet("/ready", async (AppDbContext db) =>
        {
            try
            {
                await db.Database.CanConnectAsync();
                return Results.Ok(new { status = "ready" });
            }
            catch (Exception ex)
            {
                return Results.Json(new { status = "unavailable", error = ex.Message }, statusCode: 503);
            }
        });

        group.MapGet("/live", () => Results.Ok(new { status = "alive" }));
    }
}
