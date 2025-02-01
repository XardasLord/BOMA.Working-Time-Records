using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecordAggregatedHistory : Entity<int>
{
    public DateTime Date { get; set; }
    public DateTime StartNormalizedAt { get; set; }
    public DateTime FinishNormalizedAt { get; set; }
    public DateTime StartOriginalAt { get; set; }
    public DateTime FinishOriginalAt { get; set; }
    public double WorkedMinutes { get; set; }
    public double WorkedHoursRounded { get; set; }
    public double BaseNormativeHours { get; set; }
    public double FiftyPercentageBonusHours { get; set; }
    public double HundredPercentageBonusHours { get; set; }
    public double SaturdayHours { get; set; }
    public double NightHours { get; set; }
    public EmployeeSalaryAggregatedHistory SalaryInformation { get; set; }
    public bool IsEditedManually { get; set; }
    public MissingRecordEventType? MissingRecordEventType { get; set; }
}

public class EmployeeSalaryAggregatedHistory
{
    public decimal PercentageBonusSalary { get; set; }
    
    // Base rate
    public decimal BaseSalary { get; set; }
    public decimal Base50PercentageSalary { get; set; }
    public decimal Base100PercentageSalary { get; set; }
    public decimal BaseSaturdaySalary { get; set; }
    
    // Gross
    public decimal GrossBaseSalary { get; set; }
    public decimal GrossBase50PercentageSalary { get; set; }
    public decimal GrossBase100PercentageSalary { get; set; }
    public decimal GrossBaseSaturdaySalary { get; set; }
    
    // Bonus
    public decimal BonusBaseSalary { get; set; }
    public decimal BonusBase50PercentageSalary { get; set; }
    public decimal BonusBase100PercentageSalary { get; set; }
    public decimal BonusBaseSaturdaySalary { get; set; }
    
    // Gross Sum
    public decimal GrossSumBaseSalary { get; set; }
    public decimal GrossSumBase50PercentageSalary { get; set; }
    public decimal GrossSumBase100PercentageSalary { get; set; }
    public decimal GrossSumBaseSaturdaySalary { get; set; }
    
    // Bonus Sum
    public decimal BonusSumSalary { get; set; }
    
    // Night
    public decimal NightBaseSalary { get; set; }
    public double NightWorkedHours { get; set; }
    
    // Additional
    public decimal HolidaySalary { get; set; }
    public decimal SicknessSalary { get; set; }
    public decimal AdditionalSalary { get; set; }
    
    // Compensation to min salary
    public decimal MinSalaryCompensationAmount { get; set; }
    public decimal MinSalaryCompensationFactor  { get; set; }

    public decimal FinalSumSalary { get; set; }
}