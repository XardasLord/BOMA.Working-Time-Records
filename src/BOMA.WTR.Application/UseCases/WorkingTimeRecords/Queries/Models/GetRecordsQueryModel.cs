namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

public record GetRecordsQueryModel(int Month, int Year, int? DepartmentId = null, int? ShiftId = null, string? SearchText = null);