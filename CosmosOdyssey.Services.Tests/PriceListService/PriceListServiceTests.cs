using System.Net;
using System.Text;
using System.Text.Json;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Domain.Features.Routes;
using CosmosOdyssey.Services.Services.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace CosmosOdyssey.Services.Tests.PriceListService;

[TestFixture]
public class PriceListServiceTests
{
    [SetUp]
    public void Setup()
    {
        _apiUrl = "https://localhost:5001/api/PriceList";
        _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
        _apiSettingsMock.Setup(x => x.Value).Returns(new ApiSettings { PriceListUrl = _apiUrl });
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _mediatorMock = new Mock<IMediator>();
        _priceListBuilderMock = new Mock<PriceList.IBuilder>();
        _priceListLegBuilderMock = new Mock<Leg.IBuilder>();
        _priceListRepositoryMock = new Mock<IRepository<PriceList>>();


        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _sut = new Services.PriceListService(httpClient, _apiSettingsMock.Object, _mediatorMock.Object,
            _priceListBuilderMock.Object, _priceListLegBuilderMock.Object, _priceListRepositoryMock.Object, new Mock<ILogger<Services.PriceListService>>().Object);
    }

    private string _apiUrl = null!;
    private Mock<IOptions<ApiSettings>> _apiSettingsMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<PriceList.IBuilder> _priceListBuilderMock;
    private Mock<Leg.IBuilder> _priceListLegBuilderMock;
    private Mock<IRepository<PriceList>> _priceListRepositoryMock;
    private Services.PriceListService _sut;

    [Test]
    public async Task GetLatestPriceList_Returns_Error_If_Request_Results_In_Bad_Request()
    {
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act
        var result = await _sut.GetLatestPriceList();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Query failed");
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreatePriceListCommand>(), default), Times.Never);
    }

    [Test]
    public async Task GetLatestPriceList_Returns_Error_If_Request_Results_In_Server_Error()
    {
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        // Act
        var result = await _sut.GetLatestPriceList();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Server error");
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreatePriceListCommand>(), default), Times.Never);
    }

    [Test]
    public async Task GetLatestPriceList_Returns_Error_If_PriceList_With_Given_Id_Exists()
    {
        var stringContent = Builder<PriceListResponseModel>.CreateNew().With(x => x.Id, Guid.NewGuid()).Build();
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(stringContent), Encoding.UTF8, "application/json") });


        var existingPriceList = Builder<PriceList>.CreateNew().Build();
        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingPriceList);

        // Act
        var result = await _sut.GetLatestPriceList();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Price List already exists");
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreatePriceListCommand>(), default), Times.Never);
    }

    [Test]
    public void GetLatestPriceList_Throws_If_Command_Fails()
    {
        var stringContent = Builder<PriceListResponseModel>.CreateNew().With(x => x.Legs, new[]
        {
            Builder<LegResponseModel>.CreateNew()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.RouteInfo, new RouteInfoResponseModel()
                {
                    Id = Guid.NewGuid(),
                    Distance = 2,
                    From = new LocationResponseModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "New Location"
                    },
                    To = new LocationResponseModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "New Location2"
                    }
                }).With(x => x.Providers, new[]
                {
                    Builder<ProviderResponseModel>.CreateNew()
                        .With(x => x.Id, Guid.NewGuid())
                        .With(x => x.Company, new CompanyResponseModel()
                        {
                            Id = Guid.NewGuid(),
                            Name = "New Company"
                        }).Build()
                }.ToList()).Build()
        }.ToList()).With(x => x.Id, Guid.NewGuid()).Build();
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(stringContent), Encoding.UTF8, "application/json") });


        _priceListRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PriceList?)null!);

        var leg = Builder<Leg>.CreateNew().Build();
        _priceListLegBuilderMock.Setup(x => x.WithId(It.IsAny<Guid>())).Returns(_priceListLegBuilderMock.Object);
        _priceListLegBuilderMock.Setup(x => x.WithRoute(It.IsAny<Route>())).Returns(_priceListLegBuilderMock.Object);
        _priceListLegBuilderMock.Setup(x => x.WithProviders(It.IsAny<Provider>())).Returns(_priceListLegBuilderMock.Object);
        _priceListLegBuilderMock.Setup(x => x.Build()).Returns(leg);

        var priceList = Builder<PriceList>.CreateNew().Build();
        _priceListBuilderMock.Setup(x => x.WithId(It.IsAny<Guid>())).Returns(_priceListBuilderMock.Object);
        _priceListBuilderMock.Setup(x => x.WithCreatedAt(It.IsAny<DateTime>())).Returns(_priceListBuilderMock.Object);
        _priceListBuilderMock.Setup(x => x.WithValidUntil(It.IsAny<DateTime>())).Returns(_priceListBuilderMock.Object);
        _priceListBuilderMock.Setup(x => x.WithLegs(It.IsAny<Leg>())).Returns(_priceListBuilderMock.Object);
        _priceListBuilderMock.Setup(x => x.Build()).Returns(priceList);


        _mediatorMock.Setup(x => x.Send(It.IsAny<CreatePriceListCommand>(), default)).ReturnsAsync(Result.Fail("Fail"));

        // Act && Assert
        Assert.ThrowsAsync<Exception>(() => _sut.GetLatestPriceList());
    }
}