using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.App.Features.Legs.Models;

public interface ILegListItemModelProvider
{
    IEnumerable<RouteListItemModel> Provide(PriceList priceList, ListFiltersModel filters);
}

public class LegListItemModelProvider : ILegListItemModelProvider
{
    public IEnumerable<RouteListItemModel> Provide(PriceList priceList, ListFiltersModel filters)
    {
        // Step 1: Validate input
        if (priceList == null)
            throw new ArgumentNullException(nameof(priceList));

        // Step 2: Build the graph (Adjacency List)
        var graph = new Dictionary<string, List<Leg>>();
        foreach (var leg in priceList.Legs)
        {
            if (!graph.ContainsKey(leg.Route.From.Name))
                graph[leg.Route.From.Name] = new List<Leg>();

            graph[leg.Route.From.Name].Add(leg);
        }

        // Step 3: Find all paths from `fromId` to `toId`
        var allPaths = new List<List<Leg>>();

        // Helper method to recursively find paths from start to end
        void FindPaths(string current, string destination, List<Leg> currentPath, HashSet<string> visited)
        {
            if (visited.Contains(current)) return; // Avoid cycles

            visited.Add(current);

            if (graph.ContainsKey(current))
            {
                foreach (var leg in graph[current])
                {
                    var newPath = new List<Leg>(currentPath) { leg };

                    if (leg.Route.To.Name == destination)
                    {
                        allPaths.Add(newPath); // Found a valid path
                    }
                    else
                    {
                        FindPaths(leg.Route.To.Name, destination, newPath, visited); // Recursively explore next legs
                    }
                }
            }

            visited.Remove(current);
        }

        // Step 4: Find all paths starting from `fromId` to `toId`
        var visited = new HashSet<string>();
        FindPaths(filters.From, filters.To, new List<Leg>(), visited);

        // Step 5: Transform all paths into LegListItemModel
        return allPaths.Select(path => new RouteListItemModel
        {
            PriceListId = priceList.Id,
            Routes = path.Select(leg => new RouteInfoModel
            {
                Id = leg.Id,
                From = new LegInfoModel
                {
                    Id = leg.Route.From.Id,
                    Name = leg.Route.From.Name
                },
                To = new LegInfoModel
                {
                    Id = leg.Route.To.Id,
                    Name = leg.Route.To.Name
                },
                Providers = leg.Providers.Select(provider => new ProviderInfoModel
                {
                    Id = provider.Id,
                    Company = new CompanyInfoModel
                    {
                        Id = provider.Company.Id,
                        Name = provider.Company.Name
                    },
                    Price = provider.Price,
                    FlightStart = provider.FlightStart,
                    FlightEnd = provider.FlightEnd
                }).ToArray()
            }).ToArray() // Convert the list of LegInfoModel into an array
        }).ToArray();
    }
}

public class RouteListItemModel
{
    public Guid PriceListId { get; set; }
    public RouteInfoModel[] Routes { get; set; }
}

public class RouteInfoModel
{
    public Guid Id { get; set; }
    public LegInfoModel From { get; set; }
    public LegInfoModel To { get; set; }
    public ProviderInfoModel[] Providers { get; set; }
}

public class LegInfoModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class CompanyInfoModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class ProviderInfoModel
{
    public Guid Id { get; set; }
    public CompanyInfoModel Company { get; set; }
    public double Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }
}