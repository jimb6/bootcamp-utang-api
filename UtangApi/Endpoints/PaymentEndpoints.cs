using UtangApp.Api.DTOs;
using UtangApp.Api.Services;

namespace UtangApp.Api.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments");

        group.MapGet("/", async (int? contractId, IPaymentService service) =>
        {
            var payments = await service.GetAllAsync(contractId);
            return Results.Ok(payments);
        });

        group.MapGet("/{id:int}", async (int id, IPaymentService service) =>
        {
            var payment = await service.GetByIdAsync(id);
            return payment is not null ? Results.Ok(payment) : Results.NotFound();
        });

        group.MapPost("/", async (CreatePaymentRequest request, IPaymentService service) =>
        {
            var created = await service.CreateAsync(request);
            if (created is null)
                return Results.BadRequest(new { message = "Invalid contractId." });

            return Results.Created($"/api/payments/{created.Id}", created);
        });

        group.MapDelete("/{id:int}", async (int id, IPaymentService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
