using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecord : Entity<int>
{
    public static WorkingTimeRecord Create(RecordEventType eventType, DateTime occuredAt, int groupId)
    {
        return new WorkingTimeRecord(eventType, occuredAt, groupId);
    }
    
    protected WorkingTimeRecord()
    {
    }

    private WorkingTimeRecord(RecordEventType eventType, DateTime occuredAt, int groupId)
    {
        EventType = eventType;
        OccuredAt = occuredAt;
        GroupId = groupId;
    }

    public RecordEventType EventType { get; private set; }
    
    public DateTime OccuredAt { get; private set; }
    
    public int GroupId { get; private set; } // This can be made as a value object reference
}