using Ardalis.Specification.EntityFrameworkCore;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Infrastructure.Database.Repositories;

public class AggregateRepository<T> : RepositoryBase<T>, IAggregateRepository<T> where T : class, IAggregateRoot
{
    public AggregateRepository(BomaDbContext dbContext) : base(dbContext)
    {
    }
}