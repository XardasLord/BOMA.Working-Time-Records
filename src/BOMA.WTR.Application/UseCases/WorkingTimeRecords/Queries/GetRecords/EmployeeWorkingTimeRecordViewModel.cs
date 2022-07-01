using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;

public class EmployeeWorkingTimeRecordViewModel
{
    public EmployeeViewModel Employee { get; set; }
    public IEnumerable<WorkingTimeRecordAggregatedViewModel> WorkingTimeRecordsAggregated { get; set; }
}

public class WorkingTimeRecordDetailsViewModel
{
    public RecordEventType EventType { get; set; }
    public DateTime OccudedAt { get; set; }
    public int GroupId { get; set; }
}