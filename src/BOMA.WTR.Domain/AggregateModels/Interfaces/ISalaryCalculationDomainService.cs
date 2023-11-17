using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface ISalaryCalculationDomainService
{
    public void RecalculateSalary(List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth);
}