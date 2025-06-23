using Ardalis.Specification;

namespace BOMA.WTR.Domain.AggregateModels.Setting.Specifications;

public sealed class SettingByKeySpec : Specification<Setting>, ISingleResultSpecification<Setting>
{
    public SettingByKeySpec(string key)
    {
        Query.Where(x => x.Key == key);
    }
}