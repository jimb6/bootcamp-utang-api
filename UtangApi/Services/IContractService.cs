using UtangApp.Api.DTOs;

namespace UtangApp.Api.Services;

public interface IContractService
{
    Task<List<ContractResponse>> GetAllAsync(int? borrowerId);
    Task<ContractResponse?> GetByIdAsync(int id);
    Task<ContractResponse?> CreateAsync(CreateContractRequest request);
    Task<ContractResponse?> UpdateAsync(int id, UpdateContractRequest request);
    Task<bool> DeleteAsync(int id);
}
