using UtangApp.Api.DTOs;

namespace UtangApp.Api.Services;

public interface IPaymentService
{
    Task<List<PaymentResponse>> GetAllAsync(int? contractId);
    Task<PaymentResponse?> GetByIdAsync(int id);
    Task<PaymentResponse?> CreateAsync(CreatePaymentRequest request);
    Task<bool> DeleteAsync(int id);
}
