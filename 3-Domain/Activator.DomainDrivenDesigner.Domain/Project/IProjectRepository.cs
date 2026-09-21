using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Project;

public interface IProjectRepository
{
    Task<Guid?> CreateProject(Entities.Project project);

    Task<List<Entities.Project>> RetrieveFullProjects();

    Task<Entities.Project> RetrieveProjectById(Guid projectId);

    Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId);

    Task<Guid?> UpdateRequirement(Requirement requirement);
}
