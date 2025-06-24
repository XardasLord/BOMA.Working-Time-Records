using BOMA.WTR.Domain.AggregateModels.Entities;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface ISalaryCalculationDomainService
{
    // TODO: Maybe we can somehow merge those into one function since they are doing pretty the same but on a different models
    public Task<EmployeeSalaryAggregatedHistory> RecalculateHistoricalSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        decimal minSalaryCompensationAmount,
        List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth);
    
    public Task<EmployeeSalaryAggregatedHistory> GetRecalculatedCurrentMonthSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        decimal minSalaryCompensationAmount,
        List<WorkingTimeRecordAggregatedViewModel> aggregatedCurrentRecordsForMonth);
}