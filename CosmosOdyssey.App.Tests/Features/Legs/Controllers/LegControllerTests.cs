using CosmosOdyssey.App.Features.Legs.Controllers;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CosmosOdyssey.App.Tests.Features.Legs.Controllers;

public class LegControllerTests
{
    private Mock<IRepository<PriceList>> _repositoryMock;
    private  LegController _sut;
    
    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<PriceList>>();
        _sut = new LegController();
    }

    [Test]
    public async Task ListFilters_returns_bad_request_if_no_price_lists_are_found()
    {

        _repositoryMock.Setup(x => x.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1))))
            .ReturnsAsync(new List<PriceList>());
    
        // Act
        var result = await _sut.ListFilters(_repositoryMock.Object);
    
        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
    
}