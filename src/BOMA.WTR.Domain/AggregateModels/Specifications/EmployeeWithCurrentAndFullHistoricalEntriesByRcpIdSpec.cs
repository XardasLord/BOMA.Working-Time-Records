using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpec : Specification<Employee>, ISingleResultSpecification<Employee>
{
    public EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpec(int rcpId)
    {
        Query.Where(x => x.RcpId == rcpId)
            .Include(x => x.WorkingTimeRecords);
        // .Include(x => x.WorkingTimeRecordAggregatedHistories); // TODO: BOMA-10 - long execution time because of this include of historical data 
    }
}