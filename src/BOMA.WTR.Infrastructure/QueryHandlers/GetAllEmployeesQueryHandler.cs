using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public sealed class GetAllEmployeesQueryHandler(BomaDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetAllEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    public async Task<IEnumerable<EmployeeViewModel>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var databaseQuery = dbContext.Employees
            .Include(x => x.Department)
            .AsQueryable();

        databaseQuery = ApplyFilters(query, databaseQuery);

        var employees = await databaseQuery
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
    }

    private static IQueryable<Employee> ApplyFilters(GetAllEmployeesQuery query, IQueryable<Employee> databaseQuery)
    {
        databaseQuery = databaseQuery.Where(x => x.IsActive);

        if (query.QueryModel.DepartmentId > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.DepartmentId == query.QueryModel.DepartmentId);
        }

        if (query.QueryModel.ShiftId > 0)
        {
            var shift = (ShiftType)query.QueryModel.ShiftId!;
            databaseQuery = databaseQuery.Where(x => x.JobInformation.ShiftType == shift);
        }

        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e =>
                e.Name.FirstName.Contains(query.QueryModel.SearchText) ||
                e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        return databaseQuery;
    }
}