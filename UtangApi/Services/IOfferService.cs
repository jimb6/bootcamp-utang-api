using UtangApp.Api.DTOs;

namespace UtangApp.Api.Services;

public interface IOfferService
{
    Task<List<OfferResponse>> GetAllAsync(int? borrowerId);
    Task<OfferResponse?> GetByIdAsync(int id);
    Task<OfferResponse?> CreateAsync(CreateOfferRequest request);
    Task<OfferResponse?> UpdateAsync(int id, UpdateOfferRequest request);
    Task<bool> DeleteAsync(int id);
}
