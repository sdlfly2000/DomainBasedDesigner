using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Project;

public interface IProjectPersistor
{
    Task<Guid?> CreateProject(Entities.Project project);

    Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId);

    Task<Guid?> UpdateRequirement(Requirement requirement);
}
