using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels;

public class Employee : Entity<int>, IAggregateRoot
{
    private Name _name;
    private Money _salary;
    private PercentageBonus _salaryBonusPercentage;
    private int _rcpId;
    private Department _department;
    private int _departmentId;
    private readonly List<WorkingTimeRecord> _workingTimeRecords;
    private readonly List<WorkingTimeRecordAggregatedHistory> _workingTimeRecordAggregatedHistories;

    private Employee()
    {
        _workingTimeRecords = new List<WorkingTimeRecord>();
        _workingTimeRecordAggregatedHistories = new List<WorkingTimeRecordAggregatedHistory>();
    }

    private Employee(Name name, Money salary, PercentageBonus salaryBonusPercentage, int rcpId, int departmentId) 
        : this()
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryBonusPercentage;
        _rcpId = rcpId;
        _departmentId = departmentId;
    }
    
    public Name Name => _name;
    public Money Salary => _salary;
    public PercentageBonus SalaryBonusPercentage => _salaryBonusPercentage;
    public int RcpId => _rcpId;
    public Department Department => _department;
    public int DepartmentId => _departmentId;
    public virtual IReadOnlyCollection<WorkingTimeRecord> WorkingTimeRecords => _workingTimeRecords;
    public virtual IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> WorkingTimeRecordAggregatedHistories => _workingTimeRecordAggregatedHistories;

    public static Employee Add(Name name, Money salary, PercentageBonus percentageSalaryBonus, int rcpId, int departmentId)
    {
        return new Employee(name, salary, percentageSalaryBonus, rcpId, departmentId);
    }
    
    public void UpdateData(Name name, Money salary, PercentageBonus salaryPercentageBonus, int rcpId, int departmentId)
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryPercentageBonus;
        _rcpId = rcpId;
        _departmentId = departmentId;
    }
    
    public void AddWorkingTimeRecord(WorkingTimeRecord record)
    {
        if (WorkingTimeRecords
            .Where(x => x.EventType == record.EventType)
            .Where(x => x.OccuredAt == record.OccuredAt)
            .Any(x => x.DepartmentId == record.DepartmentId))
        {
            return;
        }
        
        _workingTimeRecords.Add(record);
    }
    
    public void AddWorkingTimeRecordAggregatedHistory(WorkingTimeRecordAggregatedHistory record)
    {
        if (WorkingTimeRecordAggregatedHistories.Any(x => x.Date == record.Date))
        {
            return;
        }
        
        _workingTimeRecordAggregatedHistories.Add(record);
    }
}