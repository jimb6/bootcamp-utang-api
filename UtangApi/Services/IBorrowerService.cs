using UtangApp.Api.DTOs;

namespace UtangApp.Api.Services;

public interface IBorrowerService
{
    Task<List<BorrowerResponse>> GetAllAsync();
    Task<BorrowerResponse?> GetByIdAsync(int id);
    Task<BorrowerResponse> CreateAsync(CreateBorrowerRequest request);
    Task<BorrowerResponse?> UpdateAsync(int id, UpdateBorrowerRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}
