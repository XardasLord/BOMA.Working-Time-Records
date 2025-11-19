namespace BOMA.WTR.Application.UseCases.Reports.Models;

public class WorkingHoursReportViewModel
{
    public int EmployeesCount { get; set; }
    public double NormativeHours { get; set; }
    public double FiftyPercentageBonusHours { get; set; }
    public double HundredPercentageBonusHours { get; set; }
    public double SaturdayHours { get; set; }
}