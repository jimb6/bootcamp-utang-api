using UtangApp.Api.DTOs;

namespace UtangApp.Api.Services;

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync();
}
