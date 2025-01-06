using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Users;
using FluentResults;
using MediatR;

namespace CosmosOdyssey.Domain.Features.Reservations.Commands;

public class CreateReservationCommand : IRequest<Result>
{
    public Guid PriceListId { get; private set; }
    public Customer Name { get; private set; }

    public CreateReservationCommand(Guid priceListId, Customer name)
    {
        PriceListId = priceListId;
        Name = name;
    }

    public class Handler : IRequestHandler<CreateReservationCommand, Result>
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

        public async Task<Result> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var existingPriceList = await _priceListRepository.GetByIdAsync(command.PriceListId);
            if (existingPriceList.IsFailed) return Result.Fail("Price list not found");

            var reservation = _reservationBuilder
                .WithId(Guid.NewGuid())
                .WithPriceListId(command.PriceListId)
                .WithCustomer(command.Name)
                .Build();

            var addResult = await _reservationRepository.AddAsync(reservation);

            if (addResult.IsFailed)
            {
                return Result.Fail(addResult.Errors);
            }

            return Result.Ok();
        }
    }
}