﻿using CosmosOdyssey.Domain;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Specifications;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CosmosOdyssey.Data;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddAsync(T entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Set<T>().AddAsync(entity);
            var entry = _context.Entry(entity);
            Console.WriteLine(entry);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return Result.Fail($"An error occurred while adding the entity: {ex.Message}");
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        IQueryable<T> query = _context.Set<T>();
        query = ApplyEagerLoading(query);

        // Retrieve the entity by ID
        var entity = await query.FirstOrDefaultAsync(pl => pl.Id == id);

        return entity;
    }

    public async Task<List<T>> FindAsync(ISpecification<T> specification)
    {
        IQueryable<T> query = _context.Set<T>();
        query = ApplyEagerLoading(query);
        // Apply the filter from the specification (if provided)
        var predicate = specification.ToExpression();
        if (predicate != null) query = query.Where(predicate);

        // Execute the query and return the results
        return await query.ToListAsync();
    }

    public async Task<Result> DeleteRangeAsync(List<T> entities)
    {
        try
        {
            _context.Set<T>().RemoveRange(entities);
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while deleting the entities: {ex.Message}");
        }

        await _context.SaveChangesAsync();

        return Result.Ok();
    }


    private IQueryable<T> ApplyEagerLoading(IQueryable<T> query)
    {
        // Use reflection to include navigation properties
        var entityType = _context.Model.FindEntityType(typeof(T));

        if (entityType == null) return query;

        // Include each navigation property
        foreach (var navigation in entityType.GetNavigations()) query = query.Include(navigation.Name);

        return query;
    }
}