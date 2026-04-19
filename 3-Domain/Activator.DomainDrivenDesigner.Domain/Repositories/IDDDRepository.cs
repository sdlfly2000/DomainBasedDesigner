using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IDDDRepository
{
    Task<Guid?> CreateProject(Project project);
}
