using System.Net;
using System.Text.Json;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Services.PriceListServices.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;

namespace CosmosOdyssey.Services.PriceListServices;

public interface IPriceListService
{
    Task<Result> GetPrices();
}

public class PriceListService : IPriceListService
{
    private readonly IMediator _mediator;
    private readonly string _travelPricesUrl;
    private readonly HttpClient _httpClient;
    private readonly PriceList.IBuilder _priceListBuilder;
    private readonly PriceListLeg.IBuilder _priceListLegBuilder;

    public PriceListService(HttpClient httpClient, IOptions<ApiSettings> travelPricesUrl, IMediator mediator,
        PriceList.IBuilder priceListBuilder, PriceListLeg.IBuilder priceListLegBuilder)
    {
        _httpClient = httpClient;
        _mediator = mediator;
        _priceListBuilder = priceListBuilder;
        _priceListLegBuilder = priceListLegBuilder;
        _travelPricesUrl = travelPricesUrl.Value.PriceListUrl;
    }

    public async Task<Result> GetPrices()
    {
        var response = await _httpClient.GetAsync(_travelPricesUrl);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Fail("Bad Request(400)");
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            return Result.Fail("Internal Server Error(500)");
        }

        var responseContent = await response.Content.ReadAsStringAsync();


        var modelFromResponse = JsonSerializer.Deserialize<PriceListResponseModel>(responseContent);

        if (modelFromResponse == null)
        {
            return Result.Fail("Failed to parse price list response");
        }

        var priceListLegs = modelFromResponse.Legs.Select(x => _priceListLegBuilder
                .WithId(x.Id)
                .WithRoutes(x.RouteInfo.Select(r => r.ToDomainObject()).ToArray())
                .WithProviders(x.Providers.Select(r => r.ToDomainObject()).ToArray())
                .Build())
            .ToArray();

        var priceList = _priceListBuilder
            .WithId(modelFromResponse.Id)
            .WithValidUntil(modelFromResponse.ValidUntil)
            .WithLegs(priceListLegs)
            .Build();

        await _mediator.Publish(new CreatePriceListCommand(priceList));
        return Result.Ok();
    }
}