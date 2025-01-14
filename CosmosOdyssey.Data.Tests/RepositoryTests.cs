using CosmosOdyssey.Domain;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Specifications;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CosmosOdyssey.Data.Tests;

[TestFixture]
public class RepositoryTests
{
    private ApplicationDbContext _context;
    private IRepository<PriceList> _sut;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")  // In-memory database for unit tests
            .Options;

        _context = new ApplicationDbContext(options);

        _sut = new Repository<PriceList>(_context);  // Assuming your repository uses _context
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_Adds_PriceList_Returns_Success()
    {
        var priceList = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();

        // Act
        var result = await _sut.AddAsync(priceList);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var addedPriceList = await _context.PriceLists.FindAsync(priceList.Id);
        addedPriceList.Should().NotBeNull();
        addedPriceList.Should().BeEquivalentTo(priceList);
    }


    [Test]
    public async Task GetByIdAsync_Returns_PriceList_If_Found()
    {
        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_Returns_PriceList()
    {
        var priceList = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();

        await _context.PriceLists.AddAsync(priceList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(priceList.Id);

        // Assert
        result.Should().BeEquivalentTo(priceList);
    }

    [Test]
    public async Task FindAsync_Filters_Data_Based_On_Specification()
    {
        var priceList1 = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        var priceList2 = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        await _context.PriceLists.AddRangeAsync(priceList1, priceList2);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _sut.FindAsync(new WithAnyGivenId<PriceList>(priceList1.Id));
        
        // Assert
        result.Should().BeEquivalentTo([priceList1]);
    }

    [Test]
    public async Task DeleteRangeAsync_Deletes_Entities()
    {
        var priceList1 = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        var priceList2 = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        var priceList3 = Builder<PriceList>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        await _context.PriceLists.AddRangeAsync(priceList1, priceList2, priceList3);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _sut.DeleteRangeAsync([priceList1, priceList2]);
        var query = _context.PriceLists.AsQueryable().ToArray();
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        query.Should().NotContain([priceList1, priceList2]);
        query.Should().Contain([priceList3]);
    }
}

