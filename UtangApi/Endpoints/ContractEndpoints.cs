using UtangApp.Api.DTOs;
using UtangApp.Api.Services;

namespace UtangApp.Api.Endpoints;

public static class ContractEndpoints
{
    public static void MapContractEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/contracts");

        group.MapGet("/", async (int? borrowerId, IContractService service) =>
        {
            var contracts = await service.GetAllAsync(borrowerId);
            return Results.Ok(contracts);
        });

        group.MapGet("/{id:int}", async (int id, IContractService service) =>
        {
            var contract = await service.GetByIdAsync(id);
            return contract is not null ? Results.Ok(contract) : Results.NotFound();
        });

        group.MapPost("/", async (CreateContractRequest request, IContractService service) =>
        {
            var created = await service.CreateAsync(request);
            if (created is null)
                return Results.BadRequest(new { message = "Invalid borrowerId." });

            return Results.Created($"/api/contracts/{created.Id}", created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateContractRequest request, IContractService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IContractService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
