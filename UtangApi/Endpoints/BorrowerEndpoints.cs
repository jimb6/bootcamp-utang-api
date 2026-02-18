using UtangApp.Api.DTOs;
using UtangApp.Api.Services;

namespace UtangApp.Api.Endpoints;

public static class BorrowerEndpoints
{
    public static void MapBorrowerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/borrowers");

        group.MapGet("/", async (IBorrowerService service) =>
        {
            var borrowers = await service.GetAllAsync();
            return Results.Ok(borrowers);
        });

        group.MapGet("/{id:int}", async (int id, IBorrowerService service) =>
        {
            var borrower = await service.GetByIdAsync(id);
            return borrower is not null ? Results.Ok(borrower) : Results.NotFound();
        });

        group.MapPost("/", async (CreateBorrowerRequest request, IBorrowerService service) =>
        {
            var created = await service.CreateAsync(request);
            return Results.Created($"/api/borrowers/{created.Id}", created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateBorrowerRequest request, IBorrowerService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IBorrowerService service) =>
        {
            var (success, error) = await service.DeleteAsync(id);

            if (!success && error == "NotFound")
                return Results.NotFound();

            if (!success && error == "Conflict")
                return Results.Conflict(new { message = "Cannot delete borrower with active contracts." });

            return Results.NoContent();
        });
    }
}
