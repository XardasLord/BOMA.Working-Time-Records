using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels;

public class Employee : Entity<int>, IAggregateRoot
{
    private Name _name;
    private Money _salary;
    private PercentageBonus _percentageSalaryBonus;
    private int _rcpId;
    private readonly List<WorkingTimeRecord> _workingTimeRecords;

    private Employee()
    {
        _workingTimeRecords = new List<WorkingTimeRecord>();
    }

    private Employee(Name name, Money salary, PercentageBonus percentageSalaryBonus, int rcpId) 
        : this()
    {
        _name = name;
        _salary = salary;
        _percentageSalaryBonus = percentageSalaryBonus;
        _rcpId = rcpId;
    }
    
    public Name Name => _name;
    public Money Salary => _salary;
    public PercentageBonus PercentageSalaryBonus => _percentageSalaryBonus;
    public int RcpId => _rcpId;
    public IReadOnlyCollection<WorkingTimeRecord> WorkingTimeRecords => _workingTimeRecords;

    public static Employee Add(Name name, Money salary, PercentageBonus percentageSalaryBonus, int rcpId)
    {
        return new Employee(name, salary, percentageSalaryBonus, rcpId);
    }
    
    public void UpdateData(Name name, int rcpId)
    {
        _name = name;
        _rcpId = rcpId;
    }
    
    public void AddWorkingTimeRecord(WorkingTimeRecord record)
    {
        if (WorkingTimeRecords
            .Where(x => x.EventType == record.EventType)
            .Where(x => x.OccuredAt == record.OccuredAt)
            .Any(x => x.GroupId == record.GroupId))
        {
            return;
        }
        
        _workingTimeRecords.Add(record);
    }
}