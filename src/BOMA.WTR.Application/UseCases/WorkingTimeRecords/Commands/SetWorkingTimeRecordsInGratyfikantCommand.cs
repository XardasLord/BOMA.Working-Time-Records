using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands;

public record SetWorkingTimeRecordsInGratyfikantCommand(GetRecordsQueryModel QueryModel) : ICommand;