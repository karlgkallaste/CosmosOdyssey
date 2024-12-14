using FluentResults;
using MediatR;

namespace CosmosOdyssey.Domain.Features.PriceLists.Commands;

public class CreatePriceListCommand : IRequest<Result>
{
    public PriceList PriceList { get; private set; }

    protected CreatePriceListCommand()
    {
    }

    public CreatePriceListCommand(PriceList priceList)
    {
        PriceList = priceList;
    }

    public class Handler : IRequestHandler<CreatePriceListCommand, Result>
    {
        private readonly IPriceListRepository _priceListRepository;

        public Handler(IPriceListRepository priceListRepository)
        {
            _priceListRepository = priceListRepository;
        }

        public async Task<Result> Handle(CreatePriceListCommand command, CancellationToken cancellationToken)
        {
            var existingPriceList = await _priceListRepository.GetByIdAsync(command.PriceList.Id);
            if (existingPriceList.IsSuccess)
            {
                return Result.Fail("Price list with this id already exists");
            }

            return await _priceListRepository.AddAsync(command.PriceList);
        }
    }
}