using System.Net;
using System.Text.Json;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Services.PriceListServices.Models;
using FluentResults;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Options;

namespace CosmosOdyssey.Services.PriceListServices;

public interface IPriceListService
{
    Task<Result> GetLatestPriceList();
}

public class PriceListService : IPriceListService
{
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly PriceList.IBuilder _priceListBuilder;
    private readonly Leg.IBuilder _priceListLegBuilder;
    private readonly IRepository<PriceList> _priceListRepository;
    private readonly string _travelPricesUrl;

    public PriceListService(HttpClient httpClient, IOptions<ApiSettings> travelPricesUrl, IMediator mediator,
        PriceList.IBuilder priceListBuilder, Leg.IBuilder priceListLegBuilder, IRepository<PriceList> priceListRepository)
    {
        _httpClient = httpClient;
        _mediator = mediator;
        _priceListBuilder = priceListBuilder;
        _priceListLegBuilder = priceListLegBuilder;
        _priceListRepository = priceListRepository;
        _travelPricesUrl = travelPricesUrl.Value.PriceListUrl;
    }

    public async Task<Result> GetLatestPriceList()
    {
        var response = await _httpClient.GetAsync("https://cosmosodyssey.azurewebsites.net/api/v1.0/TravelPrices");

        if (response.StatusCode == HttpStatusCode.BadRequest) return Result.Fail("Bad Request(400)");

        if (response.StatusCode == HttpStatusCode.InternalServerError) return Result.Fail("Internal Server Error(500)");

        var responseContent = await response.Content.ReadAsStringAsync();

        var modelFromResponse = JsonSerializer.Deserialize<PriceListResponseModel>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (modelFromResponse == null) return Result.Fail("Failed to parse price list response");

        var priceListLegs = modelFromResponse.Legs.Select(x => _priceListLegBuilder
                .WithId(x.Id)
                .WithRoute(x.RouteInfo.ToDomainObject())
                .WithProviders(x.Providers.Select(r => r.ToDomainObject()).ToArray())
                .Build())
            .ToArray();

        var priceList = _priceListBuilder
            .WithId(modelFromResponse.Id)
            .WithCreatedAt(DateTime.Now)
            .WithValidUntil(modelFromResponse.ValidUntil)
            .WithLegs(priceListLegs)
            .Build();

        var existingPriceList = await _priceListRepository.GetByIdAsync(modelFromResponse.Id);

        if (existingPriceList != null )
        {
            return Result.Fail("Price List already exists");
        }
        
        var result = await _mediator.Send(new CreatePriceListCommand(priceList));

        if (result.IsFailed) throw new Exception("Failed to create price list");

        
        
        BackgroundJob.Schedule(() => GetLatestPriceList(), new DateTimeOffset(modelFromResponse.ValidUntil, DateTimeOffset.Now.Offset));
        return Result.Ok();
    }
}