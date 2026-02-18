using UtangApp.Api.Enums;

namespace UtangApp.Api.DTOs;

public record ContractResponse(
    int Id,
    int BorrowerId,
    string BorrowerFullName,
    decimal PrincipalAmount,
    decimal InterestRate,
    InterestMode InterestMode,
    TermType TermType,
    int TermCount,
    decimal LiquidationRate,
    decimal TotalAmount,
    decimal RemainingBalance,
    decimal AmountPerTerm,
    DateOnly StartDate,
    DateOnly DueDate,
    ContractStatus Status,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateContractRequest(
    int BorrowerId,
    decimal PrincipalAmount,
    decimal InterestRate,
    InterestMode InterestMode,
    TermType TermType,
    int TermCount,
    decimal? LiquidationRate,
    DateOnly StartDate,
    string? Notes);

public record UpdateContractRequest(
    int? BorrowerId,
    decimal? PrincipalAmount,
    decimal? InterestRate,
    InterestMode? InterestMode,
    TermType? TermType,
    int? TermCount,
    decimal? LiquidationRate,
    DateOnly? StartDate,
    string? Notes,
    ContractStatus? Status);
