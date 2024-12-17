using CosmosOdyssey.Domain.Specifications;
using FluentResults;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public interface IRepository<T> where T : IEntity
{
    Task<Result> AddAsync(T priceList);
    Task<Result<T>> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(ISpecification<T> specification);
}