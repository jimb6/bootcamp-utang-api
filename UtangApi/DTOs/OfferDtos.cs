using UtangApp.Api.Enums;

namespace UtangApp.Api.DTOs;

public record OfferResponse(
    int Id,
    int BorrowerId,
    string BorrowerFullName,
    decimal OfferedAmount,
    decimal InterestRate,
    int TermMonths,
    DateOnly OfferDate,
    DateOnly ExpiryDate,
    OfferStatus Status,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateOfferRequest(
    int BorrowerId,
    decimal OfferedAmount,
    decimal InterestRate,
    int TermMonths,
    DateOnly ExpiryDate,
    string? Notes);

public record UpdateOfferRequest(
    OfferStatus? Status,
    string? Notes);
