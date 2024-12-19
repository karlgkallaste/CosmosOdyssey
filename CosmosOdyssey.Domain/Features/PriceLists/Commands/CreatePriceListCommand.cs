using FluentResults;
using MediatR;

namespace CosmosOdyssey.Domain.Features.PriceLists.Commands;

public class CreatePriceListCommand : IRequest<Result>
{
    protected CreatePriceListCommand()
    {
    }

    public CreatePriceListCommand(PriceList priceList)
    {
        PriceList = priceList;
    }

    public PriceList PriceList { get; }

    public class Handler : IRequestHandler<CreatePriceListCommand, Result>
    {
        private readonly IRepository<PriceList> _priceListRepository;

        public Handler(IRepository<PriceList> priceListRepository)
        {
            _priceListRepository = priceListRepository;
        }

        public async Task<Result> Handle(CreatePriceListCommand command, CancellationToken cancellationToken)
        {
            var existingPriceList = await _priceListRepository.GetByIdAsync(command.PriceList.Id);
            if (existingPriceList.IsSuccess) return Result.Fail("Price list with this id already exists");

            var addResult = await _priceListRepository.AddAsync(command.PriceList);

            if (addResult.IsFailed) return Result.Fail(addResult.Errors);

            return Result.Ok();
        }
    }
}