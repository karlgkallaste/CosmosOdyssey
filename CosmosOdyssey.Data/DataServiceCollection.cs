using CosmosOdyssey.Data.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Data;

public static class DataServiceCollection
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services)
    {
        services.AddTransient<IPriceListRepository, PriceListRepository>();
        services.AddTransient<PriceList.IBuilder, PriceList.Builder>();
        services.AddTransient<PriceListLeg.IBuilder, PriceListLeg.Builder>();
        return services;
    }
}