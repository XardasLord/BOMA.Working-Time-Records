using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface ISalaryCalculationDomainService
{
    // TODO: Maybe we can somehow merge those into one function since they are doing pretty the same but on a different models
    public EmployeeSalaryAggregatedHistory RecalculateHistoricalSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth);
    
    public EmployeeSalaryAggregatedHistory GetRecalculatedCurrentMonthSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        List<WorkingTimeRecordAggregatedViewModel> aggregatedCurrentRecordsForMonth);
}