using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Specifications;

public sealed class EmployeeByRcpIdSpec : Specification<Employee>, ISingleResultSpecification<Employee>
{
    public EmployeeByRcpIdSpec(int rcpId)
    {
        Query.Where(x => x.RcpId == rcpId).Include(x => x.WorkingTimeRecords);
    }
}