using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.Entities;

public class WorkingTimeRecord : Entity<int>
{

    public static WorkingTimeRecord Create(RecordEventType eventType, DateTime occuredAt, int userRcpId, int groupId)
    {
        return new WorkingTimeRecord(eventType, occuredAt, userRcpId, groupId);
    }
    
    protected WorkingTimeRecord()
    {
    }

    private WorkingTimeRecord(RecordEventType eventType, DateTime occuredAt, int userRcpId, int groupId)
    {
        EventType = eventType;
        OccuredAt = occuredAt;
        UserRcpId = userRcpId;
        GroupId = groupId;
    }

    public RecordEventType EventType { get; private set; }
    
    public DateTime OccuredAt { get; private set; }

    public int UserRcpId { get; private set; } // This can be made as a value object reference
    
    public int GroupId { get; private set; } // This can be made as a value object reference
}