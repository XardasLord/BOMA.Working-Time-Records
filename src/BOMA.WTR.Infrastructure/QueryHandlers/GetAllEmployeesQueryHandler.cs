using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    private readonly BomaDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(BomaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EmployeeViewModel>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var employees = await _dbContext.Employees
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
    }
}