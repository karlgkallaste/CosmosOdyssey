using System.Collections.Immutable;
using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Reservations.Commands;
using CosmosOdyssey.Domain.Features.Routes;
using CosmosOdyssey.Domain.Features.Users;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentResults;
using Moq;

namespace CosmosOdyssey.Domain.Tests.Features.Reservations.Commands;

[TestFixture]
public class CreateReservationCommandTests
{
    private Mock<IRepository<PriceList>> _priceListRepositoryMock;
    private Mock<IRepository<Reservation>> _reservationRepositoryMock;
    private Mock<Reservation.IBuilder> _reservationBuilderMock;
    private CreateReservationCommand.Handler _sut;

    [SetUp]
    public void Setup()
    {
        _priceListRepositoryMock = new Mock<IRepository<PriceList>>();
        _reservationRepositoryMock = new Mock<IRepository<Reservation>>();
        _reservationBuilderMock = new Mock<Reservation.IBuilder>();
        _sut = new CreateReservationCommand.Handler(_priceListRepositoryMock.Object, _reservationRepositoryMock.Object,
            _reservationBuilderMock.Object);
    }

    [Test]
    public async Task Returns_failed_result_if_priceList_is_not_found()
    {
        var command = Builder<CreateReservationCommand>.CreateNew().Build();

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceListId)).ReturnsAsync((PriceList?)null!);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        _reservationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task Returns_failed_result_if_route_is_not_found_in_priceList()
    {
        var command = Builder<CreateReservationCommand>.CreateNew().With(x => x.Routes, new[]
        {
            new ReserveRoute(Guid.NewGuid(), Guid.NewGuid())
        }.ToImmutableList()).Build();

        var priceList = Builder<PriceList>
            .CreateNew()
            .With(pl => pl.Legs, new[]
            {
                Builder<Leg>.CreateNew().Build()
            }.ToList()) // No matching legs
            .Build();

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceListId))
            .ReturnsAsync(priceList);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("Route not found"));
        _reservationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task Returns_failed_result_if_company_is_not_found_for_route()
    {
        var command = Builder<CreateReservationCommand>.CreateNew().With(x => x.Routes, new[]
        {
            new ReserveRoute(Guid.NewGuid(), Guid.NewGuid())
        }.ToImmutableList()).Build();

        var priceList = Builder<PriceList>
            .CreateNew()
            .With(pl => pl.Legs, new[]
            {
                Builder<Leg>.CreateNew().With(x => x.Id, command.Routes[0].LegId).Build()
            }.ToList()) // No matching legs
            .Build();

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceListId))
            .ReturnsAsync(priceList);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("Company not found"));
        _reservationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task Returns_failed_result_if_reservation_cannot_be_saved()
    {
        var companyId = Guid.NewGuid();
        var legId = Guid.NewGuid();

        var priceList = Builder<PriceList>.CreateNew().With(x => x.Legs, new[]
        {
            Builder<Leg>.CreateNew().With(x => x.Route,
                    new Route(Guid.NewGuid(), new RouteLocation(Guid.NewGuid(), "as"),
                        new RouteLocation(Guid.NewGuid(), "2"), 23))
                .With(x => x.Id, legId)
                .With(x => x.Providers,
                    new[] { new Provider(Guid.NewGuid(), new Company(companyId, "asd"), 2, DateTime.Now, DateTime.Now) }
                        .ToList()).Build(),
        }.ToList()).Build();

        var command = new CreateReservationCommand(priceList.Id, new Customer(Guid.Empty, "First", "Last"), new[]
        {
            new ReserveRoute(companyId, legId)
        }.ToImmutableList());

        var reservation = Builder<Reservation>.CreateNew().Build();
        _reservationBuilderMock.Setup(b => b
                .WithId(It.IsAny<Guid>())
                .WithPriceListId(command.PriceListId)
                .WithCustomer(command.Customer)
                .WithRoutes(It.IsAny<List<ReservationRoute>>())
                .Build())
            .Returns(reservation);

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceListId))
            .ReturnsAsync(priceList);

        _reservationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Reservation>()))
            .ReturnsAsync(Result.Fail("Could not save reservation"));

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("Could not save reservation"));
    }

    [Test]
    public async Task Returns_ok_if_add_succeeds()
    {
        var companyId = Guid.NewGuid();
        var legId = Guid.NewGuid();

        var priceList = Builder<PriceList>.CreateNew().With(x => x.Legs, new[]
        {
            Builder<Leg>.CreateNew().With(x => x.Route,
                    new Route(Guid.NewGuid(), new RouteLocation(Guid.NewGuid(), "as"),
                        new RouteLocation(Guid.NewGuid(), "2"), 23))
                .With(x => x.Id, legId)
                .With(x => x.Providers,
                    new[] { new Provider(Guid.NewGuid(), new Company(companyId, "asd"), 2, DateTime.Now, DateTime.Now) }
                        .ToList()).Build(),
        }.ToList()).Build();

        var command = new CreateReservationCommand(priceList.Id, new Customer(Guid.Empty, "First", "Last"), new[]
        {
            new ReserveRoute(companyId, legId)
        }.ToImmutableList());

        var reservation = Builder<Reservation>.CreateNew().Build();
        _reservationBuilderMock.Setup(b => b
                .WithId(It.IsAny<Guid>())
                .WithPriceListId(command.PriceListId)
                .WithCustomer(command.Customer)
                .WithRoutes(It.IsAny<List<ReservationRoute>>())
                .Build())
            .Returns(reservation);

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceListId))
            .ReturnsAsync(priceList);

        _reservationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Reservation>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeFalse();
    }
}