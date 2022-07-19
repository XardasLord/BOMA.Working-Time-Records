using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public class GetAllWorkingTimeRecordHistoriesQueryHandler : IQueryHandler<GetAllWorkingTimeRecordHistoriesQuery, IEnumerable<EmployeeWorkingTimeRecordViewModel>>
{
    private readonly BomaDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllWorkingTimeRecordHistoriesQueryHandler(BomaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EmployeeWorkingTimeRecordViewModel>> Handle(GetAllWorkingTimeRecordHistoriesQuery query, CancellationToken cancellationToken)
    {
        var databaseQuery = _dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(w => w.Date.Year == query.QueryModel.Year)
                .Where(w => w.Date.Month == query.QueryModel.Month))
            .Where(x => x.WorkingTimeRecordAggregatedHistories
                .Where(w => w.Date.Year == query.QueryModel.Year)
                .Any(w => w.Date.Month == query.QueryModel.Month))
            .Where(x => x.DepartmentId == query.QueryModel.DepartmentId);
        
        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e => e.Name.FirstName.Contains(query.QueryModel.SearchText) || e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        var employeesWithWorkingTimeRecords = await databaseQuery.ToListAsync(cancellationToken);

        var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        {
            Employee = _mapper.Map<EmployeeViewModel>(employee), 
            WorkingTimeRecordsAggregated = _mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(employee.WorkingTimeRecordAggregatedHistories) 
        }).ToList();

        result.ForEach(x =>
        {
            x.SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(x);
        });
        
        return result;
    }
}