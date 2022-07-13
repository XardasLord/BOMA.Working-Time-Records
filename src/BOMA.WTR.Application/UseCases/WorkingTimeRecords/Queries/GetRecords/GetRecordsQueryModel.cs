namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;

public record GetRecordsQueryModel(int Month, int Year, int? GroupId = null, string? SearchText = null);