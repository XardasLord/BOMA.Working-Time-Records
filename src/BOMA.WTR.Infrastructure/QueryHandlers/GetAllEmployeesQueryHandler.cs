using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    private readonly BomaDbContext _dbContext;

    public GetAllEmployeesQueryHandler(BomaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<EmployeeViewModel>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _dbContext.Employees
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);

        // We can use AutoMapper
        return employees.Select(employee => new EmployeeViewModel
        {
            Id = employee.Id,
            FirstName = employee.Name.FirstName,
            LastName = employee.Name.LastName,
            RcpId = employee.RcpId
        }).ToList();
    }
}