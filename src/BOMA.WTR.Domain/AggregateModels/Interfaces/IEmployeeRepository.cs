namespace BOMA.WTR.Domain.AggregateModels.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetAsync(int id);
    Task<Employee> GetByRcpIdAsync(int rcpId);
    Task<bool> ExistsAsync(int rcpId);
    Task AddAsync(Employee employee);
    Task SaveChangesAsync();
}