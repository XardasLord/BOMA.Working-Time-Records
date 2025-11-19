namespace BOMA.WTR.Application.UseCases.Reports.Models;

public class SalaryReportViewModel
{
    public int EmployeesCount { get; set; }
    public decimal GrossBaseSalary { get; set; }
    public decimal BonusSalary { get; set; }
    public decimal OvertimeSalary { get; set; }
    public decimal SaturdaySalary { get; set; }
    public decimal NightSalary { get; set; }
    public decimal HolidaySalary { get; set; }
    public decimal SicknessSalary { get; set; }
    public decimal AdditionalSalary { get; set; }
    public decimal CompensationSalary { get; set; }
    public decimal FinalSalary { get; set; }
}