using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class ActiveEmployeesWithHistoryRecordsByDateSpec : Specification<Employee>
{
    public ActiveEmployeesWithHistoryRecordsByDateSpec(int year, int month)
    {
        Query
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(record => record.Date.Year == year)
                .Where(record => record.Date.Month == month))
            .Where(x => x.IsActive)
            .Where(x => x.WorkingTimeRecordAggregatedHistories.Any(w => w.Date.Year == year && w.Date.Month == month));
    }
}