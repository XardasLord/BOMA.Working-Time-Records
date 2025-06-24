using AutoMapper;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;

namespace BOMA.WTR.Infrastructure.DomainService;

public class SalaryCalculationDomainServiceHandler : ISalaryCalculationDomainService
{
    private readonly IMapper _mapper;
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _employeeWorkingTimeRecordCalculationDomainService;

    public SalaryCalculationDomainServiceHandler(
        IMapper mapper,
        IEmployeeWorkingTimeRecordCalculationDomainService employeeWorkingTimeRecordCalculationDomainService)
    {
        _mapper = mapper;
        _employeeWorkingTimeRecordCalculationDomainService = employeeWorkingTimeRecordCalculationDomainService;
    }
    
    public async Task<EmployeeSalaryAggregatedHistory> RecalculateHistoricalSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        decimal minSalaryCompensationAmount,
        List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth)
    {
        var records = _mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(aggregatedHistoryRecordsForMonth);
        
        return await RecalculateRecord(baseSalary, percentageBonusSalary, holidaySalary, sicknessSalary, additionalSalary, minSalaryCompensationAmount, records);
    }

    public async Task<EmployeeSalaryAggregatedHistory> GetRecalculatedCurrentMonthSalary(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        decimal minSalaryCompensationAmount,
        List<WorkingTimeRecordAggregatedViewModel> aggregatedCurrentRecordsForMonth)
    {
        return await RecalculateRecord(baseSalary, percentageBonusSalary, holidaySalary, sicknessSalary, additionalSalary, minSalaryCompensationAmount, aggregatedCurrentRecordsForMonth);
    }

    private async Task<EmployeeSalaryAggregatedHistory> RecalculateRecord(
        decimal baseSalary,
        decimal percentageBonusSalary,
        decimal holidaySalary,
        decimal sicknessSalary,
        decimal additionalSalary,
        decimal minSalaryCompensationAmount,
        IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> recordsForMonth)
    {
        return new EmployeeSalaryAggregatedHistory
        {
            BaseSalary = baseSalary,
            PercentageBonusSalary = percentageBonusSalary,
            Base50PercentageSalary = Math.Round(baseSalary * 1.5m, 2),
            Base100PercentageSalary = Math.Round(baseSalary * 2m, 2),
            BaseSaturdaySalary = Math.Round(baseSalary * 2m, 2),
            GrossBaseSalary = CalculateGrossBaseSalary(baseSalary, recordsForMonth),
            GrossBase50PercentageSalary = CalculateGrossBase50PercentageSalary(baseSalary, recordsForMonth),
            GrossBase100PercentageSalary = CalculateGrossBase100PercentageSalary(baseSalary, recordsForMonth),
            GrossBaseSaturdaySalary = CalculateGrossBaseSaturdaySalary(baseSalary, recordsForMonth),
            BonusBaseSalary = CalculateBonusBaseSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            BonusBase50PercentageSalary = CalculateBonusBase50PercentageSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            BonusBase100PercentageSalary = CalculateBonusBase100PercentageSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            BonusBaseSaturdaySalary = CalculateBonusBaseSaturdaySalary(baseSalary, percentageBonusSalary, recordsForMonth),
            GrossSumBaseSalary = CalculateGrossSumBaseSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            GrossSumBase50PercentageSalary = CalculateGrossSumBase50PercentageSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            GrossSumBase100PercentageSalary = CalculateGrossSumBase100PercentageSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            GrossSumBaseSaturdaySalary = CalculateGrossSumBaseSaturdaySalary(baseSalary, percentageBonusSalary, recordsForMonth),
            BonusSumSalary = CalculateBonusSumSalary(baseSalary, percentageBonusSalary, recordsForMonth),
            NightBaseSalary = await CalculateNightSumSalary(recordsForMonth),
            NightWorkedHours = CalculateAllNightWorkedHours(recordsForMonth),
            HolidaySalary = holidaySalary,
            SicknessSalary = sicknessSalary,
            AdditionalSalary = additionalSalary,
            MinSalaryCompensationFactor = 0,
            MinSalaryCompensationAmount = minSalaryCompensationAmount,
            FinalSumSalary = await CalculateFinalSumSalary(
                baseSalary, percentageBonusSalary, 
                holidaySalary, sicknessSalary, additionalSalary, minSalaryCompensationAmount,
                recordsForMonth)
        };
    }

    private static decimal CalculateGrossBaseSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(baseSalary * (decimal)records.Sum(x => x.BaseNormativeHours), 2);
    }

    private static decimal CalculateGrossBase50PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(baseSalary * 1.5m * (decimal)records.Sum(x => x.FiftyPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBase100PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.HundredPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBaseSaturdaySalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.SaturdayHours), 2);
    }

    private static decimal CalculateBonusBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(CalculateGrossBaseSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(CalculateGrossBase50PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(CalculateGrossBase100PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return Math.Round(CalculateGrossBaseSaturdaySalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateGrossSumBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossBaseSalary(baseSalary, records) + 
               CalculateBonusBaseSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateBonusBase50PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateBonusBase100PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossBaseSaturdaySalary(baseSalary, records) + 
               CalculateBonusBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateBonusSumSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateGrossBaseSaturdaySalary(baseSalary, records);
    }

    private async Task<decimal> CalculateNightSumSalary(IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        var period = records.FirstOrDefault();
        if (period is null)
            return 0;
        
        var nightFactor = await _employeeWorkingTimeRecordCalculationDomainService.GetNightFactorBonus(period.Date.Year, period.Date.Month);
        var nightWorkedHours = (decimal)CalculateAllNightWorkedHours(records);
        var nightSumSalary =  (decimal)nightFactor * nightWorkedHours;

        return Math.Round(nightSumSalary, 2);
    }

    private async Task<decimal> CalculateFinalSumSalary(
        decimal baseSalary,
        decimal bonusPercentageSalary,
        decimal holidaySalary, decimal sicknessSalary, decimal additionalSalary, decimal minSalaryCompensationAmount,
        IReadOnlyCollection<WorkingTimeRecordAggregatedViewModel> records)
    {
        return CalculateGrossSumBaseSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase50PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase100PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records) +
               await CalculateNightSumSalary(records) +
               holidaySalary +
               sicknessSalary +
               additionalSalary +
               minSalaryCompensationAmount;
    }

    private static double CalculateAllNightWorkedHours(IEnumerable<WorkingTimeRecordAggregatedViewModel> records)
    {
        return records.Sum(x => x.NightHours);
    }
}