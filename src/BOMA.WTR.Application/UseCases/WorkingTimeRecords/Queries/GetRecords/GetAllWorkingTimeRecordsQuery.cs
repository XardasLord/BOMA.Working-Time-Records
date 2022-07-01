using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;

public sealed record GetAllWorkingTimeRecordsQuery(int Month, int Year, int GroupId) : IQuery<IEnumerable<EmployeeWorkingTimeRecordViewModel>>;