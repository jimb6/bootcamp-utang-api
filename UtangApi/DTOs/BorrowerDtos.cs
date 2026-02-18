namespace UtangApp.Api.DTOs;

public record BorrowerResponse(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    DateOnly BirthDate,
    string? Email,
    string Phone,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateBorrowerRequest(
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string? Email,
    string Phone,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone);

public record UpdateBorrowerRequest(
    string? FirstName,
    string? LastName,
    DateOnly? BirthDate,
    string? Email,
    string? Phone,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone);
