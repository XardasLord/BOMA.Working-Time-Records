using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.InsertGT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using MediatR;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands;

public class SetWorkingTimeRecordsInGratyfikantCommandHandler : ICommandHandler<SetWorkingTimeRecordsInGratyfikantCommand>
{
    private readonly IMediator _mediator;
    private readonly IGratyfikantService _gratyfikantService;

    public SetWorkingTimeRecordsInGratyfikantCommandHandler(IMediator mediator, IGratyfikantService gratyfikantService)
    {
        _mediator = mediator;
        _gratyfikantService = gratyfikantService;
    }
    
    public async Task<Unit> Handle(SetWorkingTimeRecordsInGratyfikantCommand command, CancellationToken cancellationToken)
    {
        var records = await _mediator.Send(new GetAllWorkingTimeRecordsQuery(command.QueryModel), cancellationToken);
        
        await _gratyfikantService.SetWorkingHours(records);
        
        return Unit.Value;
    }
}