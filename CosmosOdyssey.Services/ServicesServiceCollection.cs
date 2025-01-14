using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Services;

public static class ServicesServiceCollection
{
    public static IServiceCollection RegisterServicesServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IPriceListService, PriceListService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePriceListCommand).Assembly));

        return services;
    }
}