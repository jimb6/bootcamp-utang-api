namespace UtangApp.Api.DTOs;

public record PaymentResponse(
    int Id,
    int ContractId,
    string BorrowerFullName,
    decimal Amount,
    DateOnly PaymentDate,
    string? ReceiptNumber,
    string? Notes,
    DateTime CreatedAt);

public record CreatePaymentRequest(
    int ContractId,
    decimal Amount,
    DateOnly PaymentDate,
    string? ReceiptNumber,
    string? Notes);
