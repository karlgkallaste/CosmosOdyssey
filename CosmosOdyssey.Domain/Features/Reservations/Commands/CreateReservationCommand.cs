using System.Collections.Immutable;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Users;
using FluentResults;
using MediatR;

namespace CosmosOdyssey.Domain.Features.Reservations.Commands;

public class CreateReservationCommand : IRequest<Result<Guid>>
{
    public Guid PriceListId { get; private set; }
    public Customer Customer { get; private set; }
    public ImmutableList<ReserveRoute> Routes { get; private set; }

    public CreateReservationCommand(Guid priceListId, Customer customer, ImmutableList<ReserveRoute> routes)
    {
        PriceListId = priceListId;
        Customer = customer;
        Routes = routes;
    }
    protected CreateReservationCommand(){}

    public class Handler : IRequestHandler<CreateReservationCommand, Result<Guid>>
    {
        private readonly IRepository<PriceList> _priceListRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly Reservation.IBuilder _reservationBuilder;

        public Handler(IRepository<PriceList> priceListRepository, IRepository<Reservation> reservationRepository,
            Reservation.IBuilder reservationBuilder)
        {
            _priceListRepository = priceListRepository;
            _reservationRepository = reservationRepository;
            _reservationBuilder = reservationBuilder;
        }

        public async Task<Result<Guid>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var existingPriceList = await _priceListRepository.GetByIdAsync(command.PriceListId);
            if (existingPriceList == null) return Result.Fail("Price list not found");

            var routes = new List<ReservationRoute>();

            foreach (var route in command.Routes)
            {
                var routeLeg = existingPriceList.Legs.FirstOrDefault(x => x.Id == route.LegId);

                if (routeLeg is null) return Result.Fail("Route not found");

                var company = routeLeg.Providers.FirstOrDefault(x => x.Company.Id == route.CompanyId);

                if (company is null) return Result.Fail("Company not found");

                routes.Add(new ReservationRoute()
                {
                    Id = routeLeg.Id,
                    Company = company.Company.Name,
                    From = routeLeg.Route.From.Name,
                    To = routeLeg.Route.To.Name,
                    Price = company.Price,
                    Arrival = company.FlightEnd,
                    Depart = company.FlightStart
                });
            }

            var reservationId = Guid.NewGuid();
            var reservation = _reservationBuilder
                .WithId(reservationId)
                .WithPriceListId(command.PriceListId)
                .WithCustomer(command.Customer)
                .WithRoutes(routes)
                .Build();

            var addResult = await _reservationRepository.AddAsync(reservation);

            if (addResult.IsFailed)
            {
                return Result.Fail(addResult.Errors);
            }
            return Result.Ok(reservationId);
        }
    }
}