using Ardalis.Specification.EntityFrameworkCore;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Infrastructure.Database.Repositories;

public class AggregateReadRepository<T> : RepositoryBase<T>, IAggregateReadRepository<T> where T : class, IAggregateRoot
{
    public AggregateReadRepository(BomaDbContext dbContext) : base(dbContext)
    {
    }
}