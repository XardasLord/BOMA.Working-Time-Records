using BOMA.WTR.Domain.SeedWork;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class Department : Entity<int>
{
    private string _name;
    
    public string Name => _name;
    
    private Department()
    {
    }
    
    private Department(int id, string name)
    {
        Id = id;
        _name = name;
    }

    public static Department Create(int id, string name)
    {
        return new Department(id, name);
    }
}