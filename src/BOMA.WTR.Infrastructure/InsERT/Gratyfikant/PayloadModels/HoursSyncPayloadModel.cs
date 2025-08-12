using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.SendToGratyfikant;

namespace BOMA.WTR.Infrastructure.InsERT.Gratyfikant.PayloadModels;

public class HoursSyncPayloadModel
{
    public DepartmentType DepartmentType { get; set; }
    public List<EmployeesSyncPayloadModel> Employees { get; set; }
}