using Microsoft.EntityFrameworkCore;
using UtangApp.Api.Data;
using UtangApp.Api.DTOs;
using UtangApp.Api.Enums;

namespace UtangApp.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        var totalBorrowers = await _db.Borrowers.AsNoTracking().CountAsync();
        var contracts = await _db.Contracts.AsNoTracking().ToListAsync();
        var totalPayments = await _db.Payments.AsNoTracking().SumAsync(p => p.Amount);

        return new DashboardSummary(
            TotalBorrowers: totalBorrowers,
            TotalContracts: contracts.Count,
            TotalLentAmount: contracts.Sum(c => c.PrincipalAmount),
            TotalOutstandingBalance: contracts.Where(c => c.Status != ContractStatus.Completed).Sum(c => c.RemainingBalance),
            TotalPaymentsReceived: totalPayments,
            ActiveContracts: contracts.Count(c => c.Status == ContractStatus.Active),
            OverdueContracts: contracts.Count(c => c.Status == ContractStatus.Overdue));
    }
}
