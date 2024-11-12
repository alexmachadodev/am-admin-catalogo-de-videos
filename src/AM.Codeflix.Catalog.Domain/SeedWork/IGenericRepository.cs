using AM.Codeflix.Catalog.Domain.Entity;

namespace AM.Codeflix.Catalog.Domain.SeedWork;

public interface IGenericRepository<in TAggregate> : IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
}