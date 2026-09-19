namespace Activator.DomainDrivenDesigner.Domain.Context;

public interface IContextRepository
{
    Task<List<Entities.Context>> RetrieveContexts(Guid projectId);

    Task<Guid> CreateContext(string name, Guid projectId);
}
