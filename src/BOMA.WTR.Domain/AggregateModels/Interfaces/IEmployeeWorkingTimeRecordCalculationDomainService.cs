using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeWorkingTimeRecordCalculationDomainService
{
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords);
    public DateTime NormalizeDateTime(RecordEventType recordEventType, DateTime dateTime);
    public double GetBaseNormativeHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded);
    public double GetFiftyPercentageBonusHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded);
    public double GetHundredPercentageBonusHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded);
    public double GetSaturdayHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded);
    public double GetNightHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized);
    public double GetNightFactorBonus(int year, int month);
}