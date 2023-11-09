using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Application.InsertGT.Gratyfikant;

public interface IGratyfikantService
{
    Task SetWorkingHours(IEnumerable<EmployeeWorkingTimeRecordViewModel> records);
}