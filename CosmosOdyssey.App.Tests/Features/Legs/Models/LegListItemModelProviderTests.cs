using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Routes;
using FizzWare.NBuilder;
using FluentAssertions;

namespace CosmosOdyssey.App.Tests.Features.Legs.Models;

[TestFixture]
public class LegListItemModelProviderTests
{
    private LegListItemModelProvider _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new LegListItemModelProvider();
    }

    [Test]
    public void Provide_returns_empty_list_when_no_routes_are_found()
    {
        var priceList = Builder<PriceList>.CreateNew().With(x => x.Legs, new[]
            {
                Builder<Leg>.CreateNew().With(x => x.Route,
                        Builder<Route>.CreateNew()
                            .With(x => x.To, new RouteLocation(Guid.NewGuid(), "Y"))
                            .With(x => x.From, new RouteLocation(Guid.NewGuid(), "Z"))
                            .Build())
                    .Build(),
                Builder<Leg>.CreateNew().With(x => x.Route,
                        Builder<Route>.CreateNew()
                            .With(x => x.To, new RouteLocation(Guid.NewGuid(), "Y"))
                            .With(x => x.From, new RouteLocation(Guid.NewGuid(), "C"))
                            .Build())
                    .Build(),
            }.ToList())
            .Build();

        var filters = Builder<ListFiltersModel>.CreateNew()
            .With(x => x.To, "Y")
            .With(x => x.From, "X")
            .Build();

        // Act
        var result = _sut.Provide(priceList, filters);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Provide_returns_list_of_all_possible_routes_when_found()
    {
        var priceList = Builder<PriceList>.CreateNew().With(x => x.Legs, new[]
            {
                Builder<Leg>.CreateNew().With(x => x.Providers, new[]
                        {
                            Builder<Provider>.CreateNew()
                                .With(x => x.Company, new Company(Guid.NewGuid(), "Comp1"))
                                .With(x => x.FlightEnd, DateTime.Now.AddDays(1)).Build()
                        }.ToList()
                    ).With(x => x.Route,
                        Builder<Route>.CreateNew()
                            .With(x => x.To, new RouteLocation(Guid.NewGuid(), "C"))
                            .With(x => x.From, new RouteLocation(Guid.NewGuid(), "A"))
                            .Build())
                    .Build(),
                Builder<Leg>.CreateNew().With(x => x.Providers, new[]
                    {
                        Builder<Provider>.CreateNew().With(x => x.Company, new Company(Guid.NewGuid(), "Comp2"))
                            .With(x => x.FlightEnd, DateTime.Now.AddDays(3))
                            .With(x => x.FlightStart, DateTime.Now.AddDays(2)).Build()
                    }.ToList()).With(x => x.Route,
                        Builder<Route>.CreateNew()
                            .With(x => x.To, new RouteLocation(Guid.NewGuid(), "E"))
                            .With(x => x.From, new RouteLocation(Guid.NewGuid(), "C"))
                            .Build())
                    .Build(),
                Builder<Leg>.CreateNew().With(x => x.Providers, new[]
                    {
                        Builder<Provider>.CreateNew().With(x => x.Company, new Company(Guid.NewGuid(), "Comp2"))
                            .With(x => x.FlightEnd, DateTime.Now.AddDays(3))
                            .With(x => x.FlightStart, DateTime.Now.AddDays(2)).Build()
                    }.ToList()).With(x => x.Route,
                        Builder<Route>.CreateNew()
                            .With(x => x.To, new RouteLocation(Guid.NewGuid(), "E"))
                            .With(x => x.From, new RouteLocation(Guid.NewGuid(), "A"))
                            .Build())
                    .Build(),
            }.ToList())
            .Build();
        var filters = Builder<ListFiltersModel>.CreateNew()
            .With(x => x.To, "E")
            .With(x => x.From, "A")
            .Build();

        // Act
        var result = _sut.Provide(priceList, filters);

        // Assert
        result.Count.Should().Be(2);
        result[0].PriceListId.Should().Be(priceList.Id);
        result[0].Routes[0].From.Name.Should().Be("A");
        result[0].Routes[0].To.Name.Should().Be("C");
        result[0].Routes[1].From.Name.Should().Be("C");
        result[0].Routes[1].To.Name.Should().Be("E");

        result[1].PriceListId.Should().Be(priceList.Id);
        result[1].Routes[0].To.Name.Should().Be("E");
        result[1].Routes[0].From.Name.Should().Be("A");
    }
}