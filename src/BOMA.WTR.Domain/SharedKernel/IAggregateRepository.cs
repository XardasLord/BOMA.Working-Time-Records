using Ardalis.Specification;

namespace BOMA.WTR.Domain.SharedKernel;

public interface IAggregateRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }