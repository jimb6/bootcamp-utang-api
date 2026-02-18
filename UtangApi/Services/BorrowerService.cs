using Microsoft.EntityFrameworkCore;
using UtangApp.Api.Data;
using UtangApp.Api.DTOs;
using UtangApp.Api.Enums;
using UtangApp.Api.Mapping;

namespace UtangApp.Api.Services;

public class BorrowerService : IBorrowerService
{
    private readonly AppDbContext _db;

    public BorrowerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<BorrowerResponse>> GetAllAsync()
    {
        var borrowers = await _db.Borrowers
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .ToListAsync();

        return borrowers.Select(b => b.ToResponse()).ToList();
    }

    public async Task<BorrowerResponse?> GetByIdAsync(int id)
    {
        var borrower = await _db.Borrowers
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        return borrower?.ToResponse();
    }

    public async Task<BorrowerResponse> CreateAsync(CreateBorrowerRequest request)
    {
        var entity = request.ToEntity();
        _db.Borrowers.Add(entity);
        await _db.SaveChangesAsync();
        return entity.ToResponse();
    }

    public async Task<BorrowerResponse?> UpdateAsync(int id, UpdateBorrowerRequest request)
    {
        var entity = await _db.Borrowers.FindAsync(id);
        if (entity is null) return null;

        entity.ApplyUpdate(request);
        await _db.SaveChangesAsync();
        return entity.ToResponse();
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var entity = await _db.Borrowers
            .Include(b => b.Contracts)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (entity is null) return (false, "NotFound");

        bool hasActiveContracts = entity.Contracts
            .Any(c => c.Status == ContractStatus.Active || c.Status == ContractStatus.Overdue);

        if (hasActiveContracts)
            return (false, "Conflict");

        _db.Borrowers.Remove(entity);
        await _db.SaveChangesAsync();
        return (true, null);
    }
}
