using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Data;

public static class DataServiceCollection
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=cosmos_odyssey;Username=cosmos;Password=odyssey"));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<PriceList.IBuilder, PriceList.Builder>();
        services.AddTransient<Leg.IBuilder, Leg.Builder>();
        return services;
    }
}