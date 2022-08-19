using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Infrastructure.Database;

namespace BOMA.WTR.Infrastructure.GraphQL.Queries;

public class BomaGraphQLQuery
{
    // [UseProjection]
    // [UseFiltering]
    // public IQueryable<EmployeeViewModel> GetEmpoloyees([Service] BomaDbContext dbContext) // TODO: This should use a new dedicated ViewContext
    //     => dbContext.Employees.AsQueryable();

    public IQueryable<string> GetTest()
        => new List<string>().AsQueryable();
}