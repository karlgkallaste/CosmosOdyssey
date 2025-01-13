using System.Net;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using FluentAssertions;
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
        result.Errors.First().Message.Should().Be("Bad Request(400)");
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
        result.Errors.First().Message.Should().Be("Internal Server Error(500)");
    }
}