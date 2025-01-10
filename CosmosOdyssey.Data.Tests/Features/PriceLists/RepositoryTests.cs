using CosmosOdyssey.Domain.Features.PriceLists;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CosmosOdyssey.Data.Tests.Features.PriceLists;

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
}