using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class EmployeeWithCurrentEntriesByRcpIdSpec : Specification<Employee>, ISingleResultSpecification<Employee>
{
    public EmployeeWithCurrentEntriesByRcpIdSpec(int rcpId)
    {
        Query.Where(x => x.RcpId == rcpId)
            .Include(x => x.WorkingTimeRecords);
    }
}