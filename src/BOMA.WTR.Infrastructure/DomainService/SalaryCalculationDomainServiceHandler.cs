using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;

namespace BOMA.WTR.Infrastructure.DomainService;

public class SalaryCalculationDomainServiceHandler : ISalaryCalculationDomainService
{
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _employeeWorkingTimeRecordCalculationDomainService;

    public SalaryCalculationDomainServiceHandler(IEmployeeWorkingTimeRecordCalculationDomainService employeeWorkingTimeRecordCalculationDomainService)
    {
        _employeeWorkingTimeRecordCalculationDomainService = employeeWorkingTimeRecordCalculationDomainService;
    }
    
    public void RecalculateSalary(List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth)
    {
        var baseSalary = aggregatedHistoryRecordsForMonth.First().SalaryInformation.BaseSalary;
        var percentageBonusSalary = aggregatedHistoryRecordsForMonth.First().SalaryInformation.PercentageBonusSalary;

        foreach (var historyRecord in aggregatedHistoryRecordsForMonth)
        {
            historyRecord.SalaryInformation.Base50PercentageSalary = Math.Round(baseSalary * 1.5m, 2);
            historyRecord.SalaryInformation.Base100PercentageSalary = Math.Round(baseSalary * 2m, 2);
            historyRecord.SalaryInformation.BaseSaturdaySalary = Math.Round(baseSalary * 2m, 2);
            
            historyRecord.SalaryInformation.GrossBaseSalary = CalculateGrossBaseSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBase50PercentageSalary = CalculateGrossBase50PercentageSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBase100PercentageSalary = CalculateGrossBase100PercentageSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBaseSaturdaySalary = CalculateGrossBaseSaturdaySalary(baseSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.BonusBaseSalary = CalculateBonusBaseSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBase50PercentageSalary = CalculateBonusBase50PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBase100PercentageSalary = CalculateBonusBase100PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBaseSaturdaySalary = CalculateBonusBaseSaturdaySalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.GrossSumBaseSalary = CalculateGrossSumBaseSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBase50PercentageSalary = CalculateGrossSumBase50PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBase100PercentageSalary = CalculateGrossSumBase100PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBaseSaturdaySalary = CalculateGrossSumBaseSaturdaySalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.BonusSumSalary = CalculateBonusSumSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.NightBaseSalary = CalculateNightSumSalary(aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.NightWorkedHours = CalculateAllNightWorkedHours(aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.FinalSumSalary = CalculateFinalSumSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
        }
    }

    private static decimal CalculateGrossBaseSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * (decimal)records.Sum(x => x.BaseNormativeHours), 2);
    }

    private static decimal CalculateGrossBase50PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 1.5m * (decimal)records.Sum(x => x.FiftyPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBase100PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.HundredPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBaseSaturdaySalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.SaturdayHours), 2);
    }

    private static decimal CalculateBonusBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBaseSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBase50PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBase100PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBaseSaturdaySalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateGrossSumBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBaseSalary(baseSalary, records) + 
               CalculateBonusBaseSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateBonusBase50PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateBonusBase100PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBaseSaturdaySalary(baseSalary, records) + 
               CalculateBonusBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateBonusSumSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateGrossBaseSaturdaySalary(baseSalary, records);
    }

    private decimal CalculateNightSumSalary(IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        var period = records.FirstOrDefault();
        if (period is null)
            return 0;
        
        var nightFactor = _employeeWorkingTimeRecordCalculationDomainService.GetNightFactorBonus(period.Date.Year, period.Date.Month);
        var nightWorkedHours = (decimal)CalculateAllNightWorkedHours(records);
        var nightSumSalary =  (decimal)nightFactor * nightWorkedHours;

        return Math.Round(nightSumSalary, 2);
    }

    private decimal CalculateFinalSumSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        var firstRecord = records.First();
        
        return CalculateGrossSumBaseSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase50PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase100PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records) +
               CalculateNightSumSalary(records) +
               firstRecord.SalaryInformation.HolidaySalary +
               firstRecord.SalaryInformation.SicknessSalary +
               firstRecord.SalaryInformation.AdditionalSalary;
    }

    private static double CalculateAllNightWorkedHours(IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return records.Sum(x => x.NightHours);
    }
}