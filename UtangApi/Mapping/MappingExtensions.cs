using UtangApp.Api.DTOs;
using UtangApp.Api.Entities;
using UtangApp.Api.Enums;

namespace UtangApp.Api.Mapping;

public static class MappingExtensions
{
    // Borrower
    public static BorrowerResponse ToResponse(this Borrower entity) =>
        new(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.FullName,
            entity.BirthDate,
            entity.Email,
            entity.Phone,
            entity.Address,
            entity.EmergencyContactName,
            entity.EmergencyContactPhone,
            entity.CreatedAt,
            entity.UpdatedAt);

    public static Borrower ToEntity(this CreateBorrowerRequest dto)
    {
        var now = DateTime.UtcNow;
        return new Borrower
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public static void ApplyUpdate(this Borrower entity, UpdateBorrowerRequest dto)
    {
        if (dto.FirstName is not null) entity.FirstName = dto.FirstName;
        if (dto.LastName is not null) entity.LastName = dto.LastName;
        if (dto.BirthDate.HasValue) entity.BirthDate = dto.BirthDate.Value;
        if (dto.Email is not null) entity.Email = dto.Email;
        if (dto.Phone is not null) entity.Phone = dto.Phone;
        if (dto.Address is not null) entity.Address = dto.Address;
        if (dto.EmergencyContactName is not null) entity.EmergencyContactName = dto.EmergencyContactName;
        if (dto.EmergencyContactPhone is not null) entity.EmergencyContactPhone = dto.EmergencyContactPhone;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    // Contract
    public static ContractResponse ToResponse(this Contract entity) =>
        new(
            entity.Id,
            entity.BorrowerId,
            entity.Borrower?.FullName ?? string.Empty,
            entity.PrincipalAmount,
            entity.InterestRate,
            entity.InterestMode,
            entity.TermType,
            entity.TermCount,
            entity.LiquidationRate,
            entity.TotalAmount,
            entity.RemainingBalance,
            entity.AmountPerTerm,
            entity.StartDate,
            entity.DueDate,
            entity.Status,
            entity.Notes,
            entity.CreatedAt,
            entity.UpdatedAt);

    public static Contract ToEntity(this CreateContractRequest dto)
    {
        var now = DateTime.UtcNow;
        decimal totalAmount = ComputeTotalAmount(dto.PrincipalAmount, dto.InterestRate, dto.InterestMode);
        decimal amountPerTerm = Math.Round(totalAmount / dto.TermCount, 2);
        DateOnly dueDate = ComputeDueDate(dto.StartDate, dto.TermCount, dto.TermType);

        return new Contract
        {
            BorrowerId = dto.BorrowerId,
            PrincipalAmount = dto.PrincipalAmount,
            InterestRate = dto.InterestRate,
            InterestMode = dto.InterestMode,
            TermType = dto.TermType,
            TermCount = dto.TermCount,
            LiquidationRate = dto.LiquidationRate ?? 0m,
            TotalAmount = totalAmount,
            RemainingBalance = totalAmount,
            AmountPerTerm = amountPerTerm,
            StartDate = dto.StartDate,
            DueDate = dueDate,
            Status = ContractStatus.Active,
            Notes = dto.Notes,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public static void ApplyUpdate(this Contract entity, UpdateContractRequest dto)
    {
        if (dto.BorrowerId.HasValue) entity.BorrowerId = dto.BorrowerId.Value;
        if (dto.PrincipalAmount.HasValue) entity.PrincipalAmount = dto.PrincipalAmount.Value;
        if (dto.InterestRate.HasValue) entity.InterestRate = dto.InterestRate.Value;
        if (dto.InterestMode.HasValue) entity.InterestMode = dto.InterestMode.Value;
        if (dto.TermType.HasValue) entity.TermType = dto.TermType.Value;
        if (dto.TermCount.HasValue) entity.TermCount = dto.TermCount.Value;
        if (dto.LiquidationRate.HasValue) entity.LiquidationRate = dto.LiquidationRate.Value;
        if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
        if (dto.Notes is not null) entity.Notes = dto.Notes;
        if (dto.Status.HasValue) entity.Status = dto.Status.Value;

        if (dto.PrincipalAmount.HasValue || dto.InterestRate.HasValue || dto.InterestMode.HasValue ||
            dto.TermCount.HasValue || dto.TermType.HasValue || dto.StartDate.HasValue)
        {
            entity.TotalAmount = ComputeTotalAmount(entity.PrincipalAmount, entity.InterestRate, entity.InterestMode);
            entity.AmountPerTerm = Math.Round(entity.TotalAmount / entity.TermCount, 2);
            entity.DueDate = ComputeDueDate(entity.StartDate, entity.TermCount, entity.TermType);
        }

        entity.UpdatedAt = DateTime.UtcNow;
    }

    // Payment
    public static PaymentResponse ToResponse(this Payment entity) =>
        new(
            entity.Id,
            entity.ContractId,
            entity.Contract?.Borrower?.FullName ?? string.Empty,
            entity.Amount,
            entity.PaymentDate,
            entity.ReceiptNumber,
            entity.Notes,
            entity.CreatedAt);

    public static Payment ToEntity(this CreatePaymentRequest dto) =>
        new()
        {
            ContractId = dto.ContractId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            ReceiptNumber = dto.ReceiptNumber,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

    // Offer
    public static OfferResponse ToResponse(this Offer entity) =>
        new(
            entity.Id,
            entity.BorrowerId,
            entity.Borrower?.FullName ?? string.Empty,
            entity.OfferedAmount,
            entity.InterestRate,
            entity.TermMonths,
            entity.OfferDate,
            entity.ExpiryDate,
            entity.Status,
            entity.Notes,
            entity.CreatedAt,
            entity.UpdatedAt);

    public static Offer ToEntity(this CreateOfferRequest dto)
    {
        var now = DateTime.UtcNow;
        return new Offer
        {
            BorrowerId = dto.BorrowerId,
            OfferedAmount = dto.OfferedAmount,
            InterestRate = dto.InterestRate,
            TermMonths = dto.TermMonths,
            OfferDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ExpiryDate = dto.ExpiryDate,
            Status = OfferStatus.Pending,
            Notes = dto.Notes,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public static void ApplyUpdate(this Offer entity, UpdateOfferRequest dto)
    {
        if (dto.Status.HasValue) entity.Status = dto.Status.Value;
        if (dto.Notes is not null) entity.Notes = dto.Notes;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    // Helpers
    public static decimal ComputeTotalAmount(decimal principal, decimal rate, InterestMode mode) =>
        mode switch
        {
            InterestMode.Simple => principal + (principal * rate / 100m),
            InterestMode.Compound => principal * (1m + rate / 100m),
            _ => principal
        };

    public static DateOnly ComputeDueDate(DateOnly startDate, int termCount, TermType termType) =>
        termType switch
        {
            TermType.Daily => startDate.AddDays(termCount),
            TermType.Weekly => startDate.AddDays(termCount * 7),
            TermType.Monthly => startDate.AddMonths(termCount),
            _ => startDate
        };
}
