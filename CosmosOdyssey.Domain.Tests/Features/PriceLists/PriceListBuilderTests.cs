using CosmosOdyssey.Domain.Features.PriceLists;
using FluentAssertions;

namespace CosmosOdyssey.Domain.Tests.Features.PriceLists;

[TestFixture]
public class PriceListBuilderTests
{
    private PriceList.IBuilder _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new PriceList.Builder();
    }

    [Test]
    public void WithId_Adds_Id_To_PriceList()
    {
        var expectedId = Guid.NewGuid();
        _sut.WithId(expectedId);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.Id.Should().Be(expectedId);
    }

    [Test]
    public void WithValidUntil_Adds_Valid_Until_To_PriceList()
    {
        var expectedDate = DateTimeOffset.UtcNow.AddDays(20);
        _sut.WithValidUntil(expectedDate);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.ValidUntil.Should().Be(expectedDate);
    }
}