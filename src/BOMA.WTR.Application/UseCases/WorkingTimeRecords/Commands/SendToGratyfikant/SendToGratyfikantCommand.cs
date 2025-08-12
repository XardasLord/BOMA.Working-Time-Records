using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.SendToGratyfikant;

public record SendToGratyfikantCommand(GetRecordsQueryModel QueryModel, DateRangeQueryModel DateRangeQueryModel) : ICommand<List<string>>;