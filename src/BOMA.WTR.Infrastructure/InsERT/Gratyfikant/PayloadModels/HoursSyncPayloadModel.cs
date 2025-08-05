namespace BOMA.WTR.Infrastructure.InsERT.Gratyfikant.PayloadModels;

public class HoursSyncPayloadModel
{
    public EmployeePayloadModel Employee { get; set; }
    public List<WorkingTimeRecordPayloadModel> WorkingTimeRecords { get; set; }
}