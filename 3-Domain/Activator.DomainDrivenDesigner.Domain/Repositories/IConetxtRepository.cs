using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IContextRepository
{
    Task<List<Context>> RetrieveContexts(Guid projectId);

    Task<Guid> CreateContext(string name, Guid projectId);
}
