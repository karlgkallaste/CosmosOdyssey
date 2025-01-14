using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Data;

public static class DataServiceCollection
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<PriceList.IBuilder, PriceList.Builder>();
        services.AddTransient<Reservation.IBuilder, Reservation.Builder>();
        services.AddTransient<Leg.IBuilder, Leg.Builder>();
        return services;
    }
}