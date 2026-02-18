using Microsoft.EntityFrameworkCore;
using UtangApp.Api.Data;
using UtangApp.Api.DTOs;
using UtangApp.Api.Mapping;

namespace UtangApp.Api.Services;

public class OfferService : IOfferService
{
    private readonly AppDbContext _db;

    public OfferService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<OfferResponse>> GetAllAsync(int? borrowerId)
    {
        var query = _db.Offers
            .AsNoTracking()
            .Include(o => o.Borrower)
            .AsQueryable();

        if (borrowerId.HasValue)
            query = query.Where(o => o.BorrowerId == borrowerId.Value);

        var offers = await query.OrderBy(o => o.Id).ToListAsync();
        return offers.Select(o => o.ToResponse()).ToList();
    }

    public async Task<OfferResponse?> GetByIdAsync(int id)
    {
        var offer = await _db.Offers
            .AsNoTracking()
            .Include(o => o.Borrower)
            .FirstOrDefaultAsync(o => o.Id == id);

        return offer?.ToResponse();
    }

    public async Task<OfferResponse?> CreateAsync(CreateOfferRequest request)
    {
        var borrowerExists = await _db.Borrowers.AnyAsync(b => b.Id == request.BorrowerId);
        if (!borrowerExists) return null;

        var entity = request.ToEntity();
        _db.Offers.Add(entity);
        await _db.SaveChangesAsync();

        await _db.Entry(entity).Reference(o => o.Borrower).LoadAsync();
        return entity.ToResponse();
    }

    public async Task<OfferResponse?> UpdateAsync(int id, UpdateOfferRequest request)
    {
        var entity = await _db.Offers
            .Include(o => o.Borrower)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (entity is null) return null;

        entity.ApplyUpdate(request);
        await _db.SaveChangesAsync();
        return entity.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Offers.FindAsync(id);
        if (entity is null) return false;

        _db.Offers.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
