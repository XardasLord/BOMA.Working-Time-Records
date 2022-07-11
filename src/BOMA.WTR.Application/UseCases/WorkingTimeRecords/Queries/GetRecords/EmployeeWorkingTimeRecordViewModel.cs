using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;

public class EmployeeWorkingTimeRecordViewModel
{
    public EmployeeViewModel Employee { get; set; }
    public EmployeeSalaryViewModel SalaryInformation { get; set; }
    public IEnumerable<WorkingTimeRecordAggregatedViewModel> WorkingTimeRecordsAggregated { get; set; }
}

public class WorkingTimeRecordDetailsViewModel
{
    public RecordEventType EventType { get; set; }
    public DateTime OccudedAt { get; set; }
    public int GroupId { get; set; }
}

public class EmployeeSalaryViewModel
{
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
}