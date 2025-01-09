using System.Collections.Immutable;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using FizzWare.NBuilder;
using FluentAssertions;

namespace CosmosOdyssey.Domain.Tests.Features.PriceLists;

[TestFixture]
public class PriceListBuilderTests
{
    [SetUp]
    public void Setup()
    {
        _sut = new PriceList.Builder();
    }

    private PriceList.IBuilder _sut;

    [Test]
    public void WithId_adds_external_id_to_priceList()
    {
        var expectedId = Guid.NewGuid();
        _sut.WithId(expectedId);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.Id.Should().Be(expectedId);
    }

    [Test]
    public void WithValidUntil_adds_valid_until_to_priceList()
    {
        var expectedDate = DateTime.UtcNow.AddDays(20);
        _sut.WithValidUntil(expectedDate);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.ValidUntil.Should().Be(expectedDate);
    }

    [Test]
    public void WithLegs_adds_legs_to_priceList()
    {
        var expectedLegs = Builder<Leg>.CreateListOfSize(3).Build().ToArray();
        _sut.WithLegs(expectedLegs);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.Legs.Should().BeEquivalentTo(expectedLegs);
    }

    [Test]
    public void WithCreatedAt_adds_crated_at_date_to_priceList()
    {
        var expectedDate = DateTime.UtcNow.AddDays(20);
        _sut.WithCreatedAt(expectedDate);

        // Act
        var priceList = _sut.Build();

        // Assert
        priceList.CreatedAt.Should().Be(expectedDate);
    }
}