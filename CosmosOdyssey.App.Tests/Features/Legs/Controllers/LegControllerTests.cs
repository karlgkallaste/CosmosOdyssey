using System.Collections.Immutable;
using CosmosOdyssey.App.Features.Legs.Controllers;
using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using CosmosOdyssey.Domain.Features.Routes;
using CosmosOdyssey.Domain.Specifications;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CosmosOdyssey.App.Tests.Features.Legs.Controllers;

public class LegControllerTests
{
    private Mock<IRepository<PriceList>> _repositoryMock;
    private Mock<ILegListItemModelProvider> _legListItemModelProviderMock;
    private Mock<IValidator<ListFiltersModel>> _listFiltersModelValidatorMock;
    private LegController _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<PriceList>>();
        _legListItemModelProviderMock = new Mock<ILegListItemModelProvider>();
        _listFiltersModelValidatorMock = new Mock<IValidator<ListFiltersModel>>();
        _sut = new LegController();
    }

    [Test]
    public async Task ListFilters_returns_bad_request_if_no_price_lists_are_found()
    {
        var specification = It.IsAny<ValidUntilNotPassed>();
        _repositoryMock.Setup(x => x.FindAsync(specification))
            .ReturnsAsync((List<PriceList>)null!);

        // Act
        var result = await _sut.ListFilters(_repositoryMock.Object);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task ListFilters_returns_routes_based_on_the_latest_priceList()
    {
        var companies = Builder<Company>.CreateListOfSize(4).Build();
        var locations = Builder<RouteLocation>.CreateListOfSize(4).Build();

        var latestPriceList = Builder<PriceList>.CreateNew()
            .With(x => x.Legs, new[]
            {
                Builder<Leg>.CreateNew()
                    .With(x => x.Route, Builder<Route>.CreateNew()
                        .With(x => x.To, new RouteLocation(locations[0].Id, locations[0].Name))
                        .With(x => x.From, new RouteLocation(locations[1].Id, locations[1].Name))
                        .Build())
                    .With(x => x.Providers, new[]
                    {
                        Builder<Provider>.CreateNew()
                            .With(x => x.Company, new Company(companies[0].Id, companies[0].Name)).Build(),
                        Builder<Provider>.CreateNew()
                            .With(x => x.Company, new Company(companies[1].Id, companies[1].Name)).Build()
                    }.ToList())
                    .Build(),
                Builder<Leg>.CreateNew()
                    .With(x => x.Route, Builder<Route>.CreateNew()
                        .With(x => x.To, new RouteLocation(locations[2].Id, locations[2].Name))
                        .With(x => x.From, new RouteLocation(locations[3].Id, locations[3].Name))
                        .Build())
                    .With(x => x.Providers, new[]
                    {
                        Builder<Provider>.CreateNew()
                            .With(x => x.Company, new Company(companies[2].Id, companies[2].Name)).Build(),
                        Builder<Provider>.CreateNew()
                            .With(x => x.Company, new Company(companies[3].Id, companies[3].Name)).Build()
                    }.ToList())
                    .Build(),
            }.ToList())
            .With(x => x.ValidUntil, DateTime.Now.AddDays(2)).Build();

        _repositoryMock.Setup(x => x.FindAsync(It.IsAny<ValidUntilNotPassed>()))
            .ReturnsAsync(new List<PriceList> { latestPriceList });

        // Act
        var result = await _sut.ListFilters(_repositoryMock.Object);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var value = (result as OkObjectResult)?.Value as LegListFilterOptionsModel;
        value.Locations.Should().BeEquivalentTo(locations.Select(x => new LocationModel(x.Id, x.Name)));
    }

    [Test]
    public async Task List_returns_badRequest_if_priceLists_are_not_found()
    {
        var filters = Builder<ListFiltersModel>.CreateNew().Build();

        var specification = It.IsAny<ValidUntilNotPassed>();
        _repositoryMock.Setup(x => x.FindAsync(specification))
            .ReturnsAsync((List<PriceList>)null!);

        // Act
        var result = await _sut.List(_legListItemModelProviderMock.Object, _listFiltersModelValidatorMock.Object,
            _repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        _legListItemModelProviderMock.Verify(x => x.Provide(It.IsAny<PriceList>(), It.IsAny<ListFiltersModel>()),
            Times.Never);
    }

    [Test]
    public async Task List_returns_badRequest_if_validation_fails()
    {
        var filters = Builder<ListFiltersModel>.CreateNew().Build();

        var priceList = Builder<PriceList>.CreateNew().Build();

        var specification = It.IsAny<ValidUntilNotPassed>();
        _repositoryMock.Setup(x => x.FindAsync(specification))
            .ReturnsAsync(new List<PriceList> { priceList });

        _listFiltersModelValidatorMock.Setup(x => x.ValidateAsync(filters, default)).ReturnsAsync(new ValidationResult
        {
            Errors =
            [
                new ValidationFailure("Error", "Message"),
            ]
        });

        // Act
        var result = await _sut.List(_legListItemModelProviderMock.Object, _listFiltersModelValidatorMock.Object,
            _repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        _legListItemModelProviderMock.Verify(x => x.Provide(It.IsAny<PriceList>(), It.IsAny<ListFiltersModel>()),
            Times.Never);
    }

    [Test]
    public async Task List_returns_ok_if_validation_succeeds()
    {
        var filters = Builder<ListFiltersModel>.CreateNew().Build();

        var priceList = Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(2)).Build();

        _repositoryMock.Setup(x => x.FindAsync(It.IsAny<ValidUntilNotPassed>()))
            .ReturnsAsync(new List<PriceList> { priceList });

        _listFiltersModelValidatorMock.Setup(x => x.ValidateAsync(filters, default))
            .ReturnsAsync(new ValidationResult());

        var routes = Builder<RouteListItemModel>.CreateListOfSize(2).Build();
        _legListItemModelProviderMock.Setup(x => x.Provide(priceList, filters))
            .Returns(routes.ToImmutableList());

        // Act
        var result = await _sut.List(_legListItemModelProviderMock.Object, _listFiltersModelValidatorMock.Object,
            _repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task LegProvidersList_returns_badRequest_if_priceList_is_not_found()
    {
        var filters = Builder<ProviderListFiltersModel>.CreateNew().Build();

        _repositoryMock.Setup(x => x.FindAsync(new WithAnyGivenId<PriceList>(filters.PriceListId)))
            .ReturnsAsync((List<PriceList>)null!);


        // Act
        var result = await _sut.Providers(_repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task LegProvidersList_returns_badRequest_if_leg_is_not_found()
    {
        var filters = Builder<ProviderListFiltersModel>.CreateNew().Build();

        var priceList = Builder<PriceList>.CreateNew().Build();
        _repositoryMock.Setup(x => x.FindAsync(new WithAnyGivenId<PriceList>(filters.PriceListId)))
            .ReturnsAsync(new List<PriceList> { priceList });

        // Act
        var result = await _sut.Providers(_repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task LegProvidersList_returns_providers_based_on_filters()
    {
        var filters = Builder<ProviderListFiltersModel>.CreateNew()
            .With(x => x.PriceListId, Guid.NewGuid())
            .With(x => x.LegId, Guid.NewGuid())
            .With(x => x.CompanyName, "Space")
            .Build();

        var priceList = Builder<PriceList>.CreateNew()
            .With(x => x.Id, filters.PriceListId)
            .With(x => x.ValidUntil, DateTime.Now).With(x => x.Legs, new[]
            {
                Builder<Leg>.CreateNew().With(x => x.Id, filters.LegId).With(x => x.Providers, new[]
                {
                    Builder<Provider>.CreateNew().With(x => x.Company, new Company(Guid.NewGuid(), "SpacE")).Build(),
                    Builder<Provider>.CreateNew().With(x => x.Company, new Company(Guid.NewGuid(), "Cosmos")).Build(),
                }.ToList()).Build()
            }.ToList()).Build();


        _repositoryMock.Setup(x => x.FindAsync(It.IsAny<WithAnyGivenId<PriceList>>()))
            .ReturnsAsync(new List<PriceList> { priceList });

        // Act
        var result = await _sut.Providers(_repositoryMock.Object, filters);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}