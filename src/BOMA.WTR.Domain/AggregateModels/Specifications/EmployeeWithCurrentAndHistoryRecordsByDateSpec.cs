using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class EmployeeWithCurrentAndHistoryRecordsByDateSpec : Specification<Employee>, ISingleResultSpecification<Employee>
{
    public EmployeeWithCurrentAndHistoryRecordsByDateSpec(int employeeId, int year, int month)
    {
        Query
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(record => record.Date.Year == year)
                .Where(record => record.Date.Month == month))
            .Include(x => x.WorkingTimeRecords
                .Where(record => record.OccuredAt.Year == year)
                .Where(record => record.OccuredAt.Month == month))
            .Where(x => x.Id == employeeId);
    }
}