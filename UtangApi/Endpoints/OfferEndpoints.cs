using UtangApp.Api.DTOs;
using UtangApp.Api.Services;

namespace UtangApp.Api.Endpoints;

public static class OfferEndpoints
{
    public static void MapOfferEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/offers");

        group.MapGet("/", async (int? borrowerId, IOfferService service) =>
        {
            var offers = await service.GetAllAsync(borrowerId);
            return Results.Ok(offers);
        });

        group.MapGet("/{id:int}", async (int id, IOfferService service) =>
        {
            var offer = await service.GetByIdAsync(id);
            return offer is not null ? Results.Ok(offer) : Results.NotFound();
        });

        group.MapPost("/", async (CreateOfferRequest request, IOfferService service) =>
        {
            var created = await service.CreateAsync(request);
            if (created is null)
                return Results.BadRequest(new { message = "Invalid borrowerId." });

            return Results.Created($"/api/offers/{created.Id}", created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateOfferRequest request, IOfferService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IOfferService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
