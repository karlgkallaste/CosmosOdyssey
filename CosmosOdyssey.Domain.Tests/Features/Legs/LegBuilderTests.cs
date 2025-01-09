using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.Routes;
using FizzWare.NBuilder;
using FluentAssertions;

namespace CosmosOdyssey.Domain.Tests.Features.Legs;

[TestFixture]
public class ReservationBuilderTests
{
    [SetUp]
    public void Setup()
    {
        _sut = new Leg.Builder();
    }

    private Leg.IBuilder _sut;

    [Test]
    public void WithRoute_adds_route_to_leg()
    {
        var expectedRoute = Builder<Route>.CreateNew().Build();
        _sut.WithRoute(expectedRoute);

        // Act
        var leg = _sut.Build();

        // Assert
        leg.Route.Should().Be(expectedRoute);
    }

    [Test]
    public void WithProviders_adds_providers_to_leg()
    {
        var expectedProviders = Builder<Provider>.CreateListOfSize(4).Build().ToArray();
        _sut.WithProviders(expectedProviders);

        // Act
        var leg = _sut.Build();

        // Assert
        leg.Providers.Should().BeEquivalentTo(expectedProviders);
    }
}

