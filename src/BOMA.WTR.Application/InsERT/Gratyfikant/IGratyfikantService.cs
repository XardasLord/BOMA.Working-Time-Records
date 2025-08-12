using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.SendToGratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Application.InsERT.Gratyfikant;

public interface IGratyfikantService
{
    Task<List<string>> SendHours(IEnumerable<EmployeeWorkingTimeRecordViewModel> records, DepartmentType departmentType);
}