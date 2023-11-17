using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

public class EmployeeWorkingTimeRecordViewModel
{
    public EmployeeViewModel Employee { get; set; }
    public EmployeeSalaryViewModel SalaryInformation { get; set; }
    public List<WorkingTimeRecordAggregatedViewModel> WorkingTimeRecordsAggregated { get; set; }
    public bool IsEditable { get; set; }
}

public class WorkingTimeRecordDetailsViewModel
{
    public RecordEventType EventType { get; set; }
    public MissingRecordEventType? MissingRecordEventType { get; set; }
    public DateTime OccudedAt { get; set; }
    public int GroupId { get; set; }
}

public class EmployeeSalaryViewModel: EmployeeSalaryAggregatedHistory
{
}