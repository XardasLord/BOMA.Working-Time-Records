namespace BOMA.WTR.Application.UseCases.Reports.Queries;

public record GetDataQueryModel(DateTime StartDate, DateTime EndDate, int? DepartmentId = null, int? ShiftId = null, string? SearchText = null);