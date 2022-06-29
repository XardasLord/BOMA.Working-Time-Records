using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.Database.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BomaDbContext _dbContext;

    public EmployeeRepository(BomaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Employee> GetByRcpIdAsync(int rcpId)
        => _dbContext.Employees
            .Include(x => x.WorkingTimeRecords)
            .SingleOrDefaultAsync(x => x.RcpId == rcpId)!;

    public Task<bool> ExistsAsync(int rcpId)
        => _dbContext.Employees.AnyAsync(x => x.RcpId == rcpId);

    public Task AddAsync(Employee employee)
    {
        _dbContext.Employees.Add(employee);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() 
        => _dbContext.SaveChangesAsync();
}