using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentResults;
using Moq;

namespace CosmosOdyssey.Domain.Tests.Features.PriceLists.Commands;

[TestFixture]
public class CreatePriceListCommandTests
{
    [SetUp]
    public void Setup()
    {
        _priceListRepositoryMock = new Mock<IRepository<PriceList>>();
        _sut = new CreatePriceListCommand.Handler(_priceListRepositoryMock.Object);
    }

    private Mock<IRepository<PriceList>> _priceListRepositoryMock;
    private CreatePriceListCommand.Handler _sut;


    [Test]
    public async Task Handle_Returns_Error_If_PriceList_With_Same_Id_Exists()
    {
        var command = Builder<CreatePriceListCommand>.CreateNew()
            .With(x => x.PriceList, Builder<PriceList>.CreateNew()
                .Build())
            .Build();

        var existingPriceList = Builder<PriceList>.CreateNew().Build();
        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceList.Id)).ReturnsAsync(existingPriceList);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Price list with this id already exists");
        _priceListRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PriceList>()), Times.Never);
    }

    [Test]
    public async Task Handle_Creates_New_PriceList()
    {
        var command = Builder<CreatePriceListCommand>.CreateNew()
            .With(x => x.PriceList, Builder<PriceList>.CreateNew()
                .Build())
            .Build();

        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(command.PriceList.Id))
            .ReturnsAsync(Result.Fail("not found"));

        _priceListRepositoryMock.Setup(x => x.AddAsync(It.IsAny<PriceList>())).ReturnsAsync(Result.Ok);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeFalse();
    }
}