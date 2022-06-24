using BOMA.WTR.Domain.Entities;
using BOMA.WTR.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WRT.Infrastructure.Database.Repositories;

public class WorkingTimeRecordRepository : IWorkingTimeRecordRepository
{
    private readonly BomaDbContext _dbContext;

    public WorkingTimeRecordRepository(BomaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsAsync(WorkingTimeRecord record)
        => _dbContext.WorkingTimeRecords
            .Where(x => x.UserRcpId == record.UserRcpId)
            .Where(x => x.EventType == record.EventType)
            .Where(x => x.OccuredAt == record.OccuredAt)
            .Where(x => x.GroupId == record.GroupId)
            .AnyAsync();

    public Task AddAsync(params WorkingTimeRecord[] records)
    {
        _dbContext.WorkingTimeRecords.AddRange(records);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}