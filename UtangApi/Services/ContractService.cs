using Microsoft.EntityFrameworkCore;
using UtangApp.Api.Data;
using UtangApp.Api.DTOs;
using UtangApp.Api.Mapping;

namespace UtangApp.Api.Services;

public class ContractService : IContractService
{
    private readonly AppDbContext _db;

    public ContractService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ContractResponse>> GetAllAsync(int? borrowerId)
    {
        var query = _db.Contracts
            .AsNoTracking()
            .Include(c => c.Borrower)
            .AsQueryable();

        if (borrowerId.HasValue)
            query = query.Where(c => c.BorrowerId == borrowerId.Value);

        var contracts = await query.OrderBy(c => c.Id).ToListAsync();
        return contracts.Select(c => c.ToResponse()).ToList();
    }

    public async Task<ContractResponse?> GetByIdAsync(int id)
    {
        var contract = await _db.Contracts
            .AsNoTracking()
            .Include(c => c.Borrower)
            .FirstOrDefaultAsync(c => c.Id == id);

        return contract?.ToResponse();
    }

    public async Task<ContractResponse?> CreateAsync(CreateContractRequest request)
    {
        var borrowerExists = await _db.Borrowers.AnyAsync(b => b.Id == request.BorrowerId);
        if (!borrowerExists) return null;

        var entity = request.ToEntity();
        _db.Contracts.Add(entity);
        await _db.SaveChangesAsync();

        await _db.Entry(entity).Reference(c => c.Borrower).LoadAsync();
        return entity.ToResponse();
    }

    public async Task<ContractResponse?> UpdateAsync(int id, UpdateContractRequest request)
    {
        var entity = await _db.Contracts
            .Include(c => c.Borrower)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null) return null;

        if (request.BorrowerId.HasValue)
        {
            var borrowerExists = await _db.Borrowers.AnyAsync(b => b.Id == request.BorrowerId.Value);
            if (!borrowerExists) return null;
        }

        entity.ApplyUpdate(request);
        await _db.SaveChangesAsync();

        if (request.BorrowerId.HasValue)
            await _db.Entry(entity).Reference(c => c.Borrower).LoadAsync();

        return entity.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Contracts.FindAsync(id);
        if (entity is null) return false;

        _db.Contracts.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
