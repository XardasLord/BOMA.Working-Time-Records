namespace BOMA.WTR.Domain.SeedWork;

public abstract class Entity<T>
{
    public T Id { get; internal set; }
}