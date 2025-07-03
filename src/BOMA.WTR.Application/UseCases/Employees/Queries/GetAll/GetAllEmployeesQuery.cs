using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQuery(GetRecordsQueryModel QueryModel) : IQuery<IEnumerable<EmployeeViewModel>>;

public record GetRecordsQueryModel(int? DepartmentId = null, int? ShiftId = null, string? SearchText = null);