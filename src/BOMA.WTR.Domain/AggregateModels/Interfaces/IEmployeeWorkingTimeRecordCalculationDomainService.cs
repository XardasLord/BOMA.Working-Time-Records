using BOMA.WTR.Domain.AggregateModels.Entities;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeWorkingTimeRecordCalculationDomainService
{
    public IEnumerable<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords);
}