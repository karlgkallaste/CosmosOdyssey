using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.App.Features.Legs.Models;

public interface ILegListItemModelProvider
{
    IEnumerable<LegListItemModel> Provide(PriceList priceList);
    IEnumerable<LegListItemModel> ProvideSpecific(PriceList priceList, Guid From, Guid To);
}

public class LegListItemModelProvider : ILegListItemModelProvider
{
    public IEnumerable<LegListItemModel> Provide(PriceList priceList)
    {
        foreach (var leg in priceList.Legs)
        {
            yield return new LegListItemModel()
            {
                From = new RouteInfoModel()
                {
                    Id = leg.Route.From.Id,
                    Name = leg.Route.From.Name,
                },
                To = new RouteInfoModel()
                {
                    Id = leg.Route.To.Id,
                    Name = leg.Route.To.Name,
                },
                Providers = leg.Providers.Select(x => new ProviderInfoModel()
                {
                    Id = x.Id,
                    Company = new CompanyInfoModel()
                    {
                        Id = x.Company.Id,
                        Name = x.Company.Name,
                    },
                    FlightEnd = x.FlightEnd,
                    FlightStart = x.FlightStart,
                    Price = x.Price
                }).ToArray()
            };
        }
    }

    public IEnumerable<LegListItemModel> ProvideSpecific(PriceList priceList, Guid fromId, Guid toId)
    {
        // Initialize the adjacency list to represent the graph (fromId -> toId connections with Providers)
        var graph = new Dictionary<Guid, List<(Guid ToId, List<ProviderInfoModel> Providers)>>();

        // Example provider info (replace this with actual provider data)
        var providers = new List<ProviderInfoModel>
        {
            new ProviderInfoModel { Company = "Airline A",  = "Flight AA123" },
            new ProviderInfoModel { Company = "Airline B", ProviderDetails = "Flight BB456" }
        };

        // Example flight connections (replace this with actual flight data)
        graph.Add(fromId, new List<(Guid, List<ProviderInfoModel>)>
        {
            (toId, providers)  // A -> B (fromId -> toId)
        });

        // To store all the valid legs
        List<LegListItemModel> validLegs = new List<LegListItemModel>();
        
        // To track visited nodes and prevent cycles
        HashSet<Guid> visited = new HashSet<Guid>();
        
        // Queue to handle the BFS traversal (path is a list of location IDs from start to current point)
        Queue<List<Guid>> queue = new Queue<List<Guid>>();
        queue.Enqueue(new List<Guid> { fromId });

        // Perform BFS to explore all possible paths from 'From' to 'To'
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var current = path.Last();

            // If we reach the destination, create the LegListItemModel for this path
            if (current == toId)
            {
                // Traverse the path and create LegListItemModel
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var fromLocation = path[i];
                    var toLocation = path[i + 1];

                    // Fetch the providers for the current leg
                    var legInfo = graph[fromLocation]
                        .FirstOrDefault(leg => leg.ToId == toLocation);
                    
                    if (legInfo != default)
                    {
                        // Construct RouteInfoModel for From and To locations
                        var fromModel = new RouteInfoModel
                        {
                            LocationId = fromLocation,
                            LocationName = $"Location {fromLocation}" // Replace with actual name lookup
                        };
                        
                        var toModel = new RouteInfoModel
                        {
                            LocationId = toLocation,
                            LocationName = $"Location {toLocation}" // Replace with actual name lookup
                        };

                        // Create LegListItemModel for this leg
                        var legItem = new LegListItemModel
                        {
                            From = fromModel,
                            To = toModel,
                            Providers = legInfo.Providers.ToArray()  // Include the provider info
                        };

                        validLegs.Add(legItem);
                    }
                }
            }
            else
            {
                // Explore the neighbors of the current location
                foreach (var neighbor in graph[current])
                {
                    if (!visited.Contains(neighbor.ToId))
                    {
                        visited.Add(neighbor.ToId);
                        var newPath = new List<Guid>(path) { neighbor.ToId };
                        queue.Enqueue(newPath);
                    }
                }
            }
        }

        return validLegs;  // Return the valid legs as an IEnumerable<LegListItemModel>
    }
}

public class LegListItemModel
{
    public RouteInfoModel From { get; set; }
    public RouteInfoModel To { get; set; }
    public ProviderInfoModel[] Providers { get; set; }
}

public class RouteInfoModel
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