using BOMA.WTR.Domain.SharedKernel;
using CsvHelper.Configuration.Attributes;

namespace BOMA.WTR.Application.RogerFiles;

public class RogerFileModel
{
    [Index(0)]
    public string? RogerEventName { get; set; }
    
    [Index(1)]
    public RecordEventType? RogerEventType { get; set; }
    
    [Index(2)]
    public string? UserId { get; set; }
    
    [Index(3)]
    public string? Name { get; set; }
    
    [Index(4)]
    public string? LastName { get; set; }
    
    [Index(5)]
    public int? UserRcpId { get; set; }
    
    [Index(6)]
    public DateTime? Date { get; set; }
    
    [Index(7)]
    public TimeSpan? Time { get; set; }
    
    [Index(8)]
    public int? GroupId { get; set; }
    
    [Index(9)]
    public string? GroupName { get; set; }

    public bool IsValid() => RogerEventType.HasValue && RogerEventType.Value is not RecordEventType.None && !string.IsNullOrEmpty(UserId);
}