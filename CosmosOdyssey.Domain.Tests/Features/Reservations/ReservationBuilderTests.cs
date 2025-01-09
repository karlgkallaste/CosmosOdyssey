using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Users;
using FizzWare.NBuilder;
using FluentAssertions;

namespace CosmosOdyssey.Domain.Tests.Features.Reservations;

[TestFixture]
public class ReservationBuilderTests
{
    [SetUp]
    public void Setup()
    {
        _sut = new Reservation.Builder();
    }

    private Reservation.IBuilder _sut;

    [Test]
    public void WithId_adds_id_to_reservation()
    {
        var expectedId = Guid.NewGuid();
        _sut.WithId(expectedId);

        // Act
        var reservation = _sut.Build();

        // Assert
        reservation.Id.Should().Be(expectedId);
    }
    
    
    [Test]
    public void WithPriceListId_adds_priceList_id_to_reservation()
    {
        var expectedId = Guid.NewGuid();
        _sut.WithPriceListId(expectedId);

        // Act
        var reservation = _sut.Build();

        // Assert
        reservation.PriceListId.Should().Be(expectedId);
    }

    [Test]
    public void WithCustomer_adds_customer_to_reservation()
    {
        var expectedCustomer = Builder<Customer>.CreateNew().Build();
        _sut.WithCustomer(expectedCustomer);

        // Act
        var reservation = _sut.Build();

        // Assert
        reservation.Customer.Should().Be(expectedCustomer);
    }

    [Test]
    public void WithRoutes_adds_route_to_reservation()
    {
        var expectedRoutes = Builder<ReservationRoute>.CreateListOfSize(2).Build();
        _sut.WithRoutes(expectedRoutes);

        // Act
        var reservation = _sut.Build();

        // Assert
        reservation.Routes.Should().BeEquivalentTo(expectedRoutes);
    }
}