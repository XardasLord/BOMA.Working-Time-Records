using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;

public sealed record GetAllWorkingTimeRecordsQuery(GetRecordsQueryModel QueryModel) : IQuery<IEnumerable<EmployeeWorkingTimeRecordViewModel>>;