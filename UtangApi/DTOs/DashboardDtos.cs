namespace UtangApp.Api.DTOs;

public record DashboardSummary(
    int TotalBorrowers,
    int TotalContracts,
    decimal TotalLentAmount,
    decimal TotalOutstandingBalance,
    decimal TotalPaymentsReceived,
    int ActiveContracts,
    int OverdueContracts);
