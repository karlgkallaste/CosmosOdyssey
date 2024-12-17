using System.Net;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Services.PriceListServices.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CosmosOdyssey.Services.Tests.PriceListService;

[TestFixture]
public class PriceListServiceTests
{
    private string _apiUrl = null!;
    private Mock<IOptions<ApiSettings>> _apiSettingsMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<PriceList.IBuilder> _priceListBuilderMock;
    private Mock<PriceListLeg.IBuilder> _priceListLegBuilderMock;
    private PriceListServices.PriceListService _sut;

    [SetUp]
    public void Setup()
    {
        _apiUrl = "https://localhost:5001/api/PriceList";
        _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
        _apiSettingsMock.Setup(x => x.Value).Returns(new ApiSettings { PriceListUrl = _apiUrl });
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _mediatorMock = new Mock<IMediator>();
        _priceListBuilderMock = new Mock<PriceList.IBuilder>();
        _priceListLegBuilderMock = new Mock<PriceListLeg.IBuilder>();

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _sut = new PriceListServices.PriceListService(httpClient, _apiSettingsMock.Object, _mediatorMock.Object,
            _priceListBuilderMock.Object, _priceListLegBuilderMock.Object);
    }

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