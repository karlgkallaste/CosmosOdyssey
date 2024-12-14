using FluentResults;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public interface IPriceListRepository
{
    Task<Result> AddAsync(PriceList priceList);
    Task<Result<PriceList>> GetByIdAsync(Guid id);
}

