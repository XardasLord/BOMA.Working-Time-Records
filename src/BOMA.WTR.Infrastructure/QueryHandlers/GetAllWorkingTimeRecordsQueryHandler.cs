﻿using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public class GetAllWorkingTimeRecordsQueryHandler : IQueryHandler<GetAllWorkingTimeRecordsQuery, IEnumerable<EmployeeWorkingTimeRecordViewModel>>
{
    private readonly BomaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _employeeWorkingTimeRecordCalculationDomainService;

    public GetAllWorkingTimeRecordsQueryHandler(
        BomaDbContext dbContext,
        IMapper mapper,
        IEmployeeWorkingTimeRecordCalculationDomainService employeeWorkingTimeRecordCalculationDomainService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _employeeWorkingTimeRecordCalculationDomainService = employeeWorkingTimeRecordCalculationDomainService;
    }
    
    public async Task<IEnumerable<EmployeeWorkingTimeRecordViewModel>> Handle(GetAllWorkingTimeRecordsQuery query, CancellationToken cancellationToken)
    {
        var databaseQuery = _dbContext.Employees
            .Include(x => x.WorkingTimeRecords
                .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                .Where(w => w.OccuredAt.Month == query.QueryModel.Month)
                .Where(w => w.GroupId == query.QueryModel.GroupId))
            .Where(x => x.WorkingTimeRecords
                .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                .Where(w => w.OccuredAt.Month == query.QueryModel.Month)
                .Any(w => w.GroupId == query.QueryModel.GroupId));

        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e => e.Name.FirstName.Contains(query.QueryModel.SearchText) || e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        var employeesWithWorkingTimeRecords = await databaseQuery.ToListAsync(cancellationToken);


        var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        {
            Employee = _mapper.Map<EmployeeViewModel>(employee), 
            WorkingTimeRecordsAggregated = _employeeWorkingTimeRecordCalculationDomainService.CalculateAggregatedWorkingTimeRecords(employee.WorkingTimeRecords)
        }).ToList();

        result.ForEach(x =>
        {
            x.SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(x);
        });
        
        return result;
    }
}