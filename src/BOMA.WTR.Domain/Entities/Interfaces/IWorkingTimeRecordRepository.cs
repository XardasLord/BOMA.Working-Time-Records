namespace BOMA.WTR.Domain.Entities.Interfaces;

public interface IWorkingTimeRecordRepository
{
    Task<bool> ExistsAsync(WorkingTimeRecord record);
    Task AddAsync(params WorkingTimeRecord[] records);
    Task SaveChangesAsync();
}