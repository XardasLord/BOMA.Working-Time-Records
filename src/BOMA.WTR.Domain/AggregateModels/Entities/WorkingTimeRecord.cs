using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecord : Entity<int>
{
    public static WorkingTimeRecord Create(RecordEventType eventType, DateTime occuredAt, int departmentId)
    {
        return new WorkingTimeRecord(eventType, occuredAt, departmentId);
    }
    
    protected WorkingTimeRecord()
    {
    }

    private WorkingTimeRecord(RecordEventType eventType, DateTime occuredAt, int departmentId)
    {
        EventType = eventType;
        OccuredAt = occuredAt;
        DepartmentId = departmentId;
    }

    public RecordEventType EventType { get; private set; }
    
    public DateTime OccuredAt { get; internal set; }
    
    public Department Department { get; private set; }
    public int DepartmentId { get; private set; }
    
    public MissingRecordEventType? MissingRecordEventType { get; private set; }
}