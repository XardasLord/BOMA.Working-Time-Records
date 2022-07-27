using BOMA.WTR.Domain.AggregateModels.Entities;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeWorkingTimeRecordCalculationDomainService
{
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords);
    public double GetBaseNormativeHours(DateTime date, double workedHoursRounded);
    public double GetFiftyPercentageBonusHours(DateTime date, double workedHoursRounded);
    public double GetHundredPercentageBonusHours(DateTime date, double workedHoursRounded);
    public double GetSaturdayHours(DateTime date, double workedHoursRounded);
    public double GetNightFactorBonus(int year, int month);
}