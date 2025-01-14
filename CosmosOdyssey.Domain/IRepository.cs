using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Specifications;
using FluentResults;

namespace CosmosOdyssey.Domain;

public interface IRepository<T> where T : IEntity
{
    Task<Result> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> FindAsync(ISpecification<T> specification);
    Task<Result> DeleteRangeAsync(List<T> entities);
}