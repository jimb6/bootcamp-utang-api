using Microsoft.EntityFrameworkCore;
using UtangApp.Api.Data;
using UtangApp.Api.DTOs;
using UtangApp.Api.Enums;
using UtangApp.Api.Mapping;

namespace UtangApp.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<PaymentResponse>> GetAllAsync(int? contractId)
    {
        var query = _db.Payments
            .AsNoTracking()
            .Include(p => p.Contract)
                .ThenInclude(c => c.Borrower)
            .AsQueryable();

        if (contractId.HasValue)
            query = query.Where(p => p.ContractId == contractId.Value);

        var payments = await query.OrderBy(p => p.Id).ToListAsync();
        return payments.Select(p => p.ToResponse()).ToList();
    }

    public async Task<PaymentResponse?> GetByIdAsync(int id)
    {
        var payment = await _db.Payments
            .AsNoTracking()
            .Include(p => p.Contract)
                .ThenInclude(c => c.Borrower)
            .FirstOrDefaultAsync(p => p.Id == id);

        return payment?.ToResponse();
    }

    public async Task<PaymentResponse?> CreateAsync(CreatePaymentRequest request)
    {
        var contract = await _db.Contracts
            .Include(c => c.Borrower)
            .FirstOrDefaultAsync(c => c.Id == request.ContractId);

        if (contract is null) return null;

        var entity = request.ToEntity();
        _db.Payments.Add(entity);

        contract.RemainingBalance -= request.Amount;
        if (contract.RemainingBalance <= 0)
            contract.Status = ContractStatus.Completed;
        contract.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        entity.Contract = contract;
        return entity.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Payments
            .Include(p => p.Contract)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null) return false;

        entity.Contract.RemainingBalance += entity.Amount;
        if (entity.Contract.RemainingBalance > 0 && entity.Contract.Status == ContractStatus.Completed)
            entity.Contract.Status = ContractStatus.Active;
        entity.Contract.UpdatedAt = DateTime.UtcNow;

        _db.Payments.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
