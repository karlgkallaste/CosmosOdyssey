using CosmosOdyssey.Domain.Features.PriceLists;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CosmosOdyssey.Data.Features.PriceLists;

public class PriceListRepository : IPriceListRepository
{
    private readonly ApplicationDbContext _context;

    public PriceListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddAsync(PriceList priceList)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.PriceLists.AddAsync(priceList);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Ok();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Result<PriceList>> GetByIdAsync(Guid id)
    {
        var priceList = await _context.PriceLists
            .FirstOrDefaultAsync(pl => pl.Id == id);

        return priceList == null
            ? Result.Fail<PriceList>("Price List not found")
            : Result.Ok(priceList);
    }

    public async Task<Result<PriceList>> GetLatest()
    {
        throw new NotImplementedException();
    }
}