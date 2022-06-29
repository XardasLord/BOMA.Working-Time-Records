using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQuery : IQuery<IEnumerable<EmployeeViewModel>>;