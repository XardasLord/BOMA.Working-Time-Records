using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;

public sealed record GetAllWorkingTimeRecordsQuery(GetRecordsQueryModel QueryModel) : IQuery<IEnumerable<EmployeeWorkingTimeRecordViewModel>>;