using CosmosOdyssey.Domain.Specifications;
using FluentResults;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public interface IRepository<T> where T : IEntity
{
    Task<Result> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> FindAsync(ISpecification<T> specification);
}