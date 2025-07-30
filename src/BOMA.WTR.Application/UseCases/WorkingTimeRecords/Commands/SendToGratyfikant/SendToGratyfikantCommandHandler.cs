using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.InsERT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using MediatR;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.SendToGratyfikant;

public class SendToGratyfikantCommandHandler(
    IMediator mediator,
    IGratyfikantService gratyfikantService) : ICommandHandler<SendToGratyfikantCommand, List<string>>
{
    public async Task<List<string>> Handle(SendToGratyfikantCommand command, CancellationToken cancellationToken)
    {
        var records = await mediator.Send(new GetAllWorkingTimeRecordsQuery(command.QueryModel), cancellationToken);
        
        var response = await gratyfikantService.SendHours(records);
        
        return response;
    }
}