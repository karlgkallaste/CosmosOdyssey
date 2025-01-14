using System.Net;
using System.Text.Json;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Domain.Specifications;
using CosmosOdyssey.Services.Services.Models;
using FluentResults;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosOdyssey.Services.Services;

public interface IPriceListService
{
    Task<Result> GetLatestPriceList();
    Task<Result> DeleteExcess();
}

public class PriceListService : IPriceListService
{
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly PriceList.IBuilder _priceListBuilder;
    private readonly Leg.IBuilder _priceListLegBuilder;
    private readonly IRepository<PriceList> _priceListRepository;
    private readonly ILogger<PriceListService> _logger;
    private readonly IOptions<ApiSettings> _apiSettings;

    public PriceListService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IMediator mediator,
        PriceList.IBuilder priceListBuilder, Leg.IBuilder priceListLegBuilder,
        IRepository<PriceList> priceListRepository, ILogger<PriceListService> logger)
    {
        _httpClient = httpClient;
        _mediator = mediator;
        _priceListBuilder = priceListBuilder;
        _priceListLegBuilder = priceListLegBuilder;
        _priceListRepository = priceListRepository;
        _logger = logger;
        _apiSettings = apiSettings;
    }

    public async Task<Result> GetLatestPriceList()
    {
        var response = await _httpClient.GetAsync(_apiSettings.Value.PriceListUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError("[{Name}] Response status code: {StatusCode}. Content: {Content}",
                nameof(GetLatestPriceList), response.StatusCode, responseContent);
            return Result.Fail("Query failed");
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError("[{Name}] Response status code: {StatusCode}. Content: {Content}",
                nameof(GetLatestPriceList), response.StatusCode, responseContent);
            return Result.Fail("Server error");
        }

        var modelFromResponse = JsonSerializer.Deserialize<PriceListResponseModel>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (modelFromResponse == null)
        {
            _logger.LogError("[{Name}] Failed to deserialize. Content: {Content}", nameof(GetLatestPriceList),
                responseContent);
            return Result.Fail("Failed to deserialize");
        }

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

        if (existingPriceList != null)
        {
            _logger.LogError("[{Name}] Tried to create duplicate price list {Id}", nameof(GetLatestPriceList),
                modelFromResponse.Id);
            return Result.Fail("Price List already exists");
        }

        var result = await _mediator.Send(new CreatePriceListCommand(priceList));
        if (result.IsFailed) throw new Exception("Failed to create price list");

        BackgroundJob.Schedule(() => GetLatestPriceList(), new DateTimeOffset(modelFromResponse.ValidUntil));
        return Result.Ok();
    }

    public async Task<Result> DeleteExcess()
    {
        var priceLists = await _priceListRepository.FindAsync(Specification<PriceList>.None);
        if (priceLists.Count <= 15)
        {
            return Result.Ok();
        }

        var priceListsToDelete = priceLists
            .OrderBy(pl => pl.ValidUntil)
            .Take(priceLists.Count - 15)
            .ToList();
        var result = await _priceListRepository.DeleteRangeAsync(priceListsToDelete);
        if (result.IsFailed)
        {
            _logger.LogError("Failed to delete price lists");
            return Result.Fail(result.Errors);
        }

        return Result.Ok();
    }
}