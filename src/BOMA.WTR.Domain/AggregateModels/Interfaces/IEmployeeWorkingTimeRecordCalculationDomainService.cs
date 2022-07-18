using BOMA.WTR.Domain.AggregateModels.Entities;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeWorkingTimeRecordCalculationDomainService
{
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords);
}