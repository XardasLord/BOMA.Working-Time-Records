using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeWorkingTimeRecordCalculationDomainService
{
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords);
    public DateTime NormalizeDateTime(RecordEventType recordEventType, DateTime dateTime);
    public double GetBaseNormativeHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded);
    public double GetFiftyPercentageBonusHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded);
    public double GetHundredPercentageBonusHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded);
    public double GetSaturdayHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded);
    public double GetNightFactorBonus(int year, int month);
}