using Ardalis.Specification;

namespace BOMA.WTR.Domain.SharedKernel;

public interface IAggregateReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }