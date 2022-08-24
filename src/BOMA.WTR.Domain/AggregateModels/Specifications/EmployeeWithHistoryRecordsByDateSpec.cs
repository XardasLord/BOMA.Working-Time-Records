using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class EmployeeWithHistoryRecordsByDateSpec : Specification<Employee>, ISingleResultSpecification<Employee>
{
    public EmployeeWithHistoryRecordsByDateSpec(int employeeId, int year, int month)
    {
        Query
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(record => record.Date.Year == year)
                .Where(record => record.Date.Month == month))
            .Where(x => x.Id == employeeId);
    }
}