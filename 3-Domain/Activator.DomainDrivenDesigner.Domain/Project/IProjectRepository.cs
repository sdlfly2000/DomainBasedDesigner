using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Project;

public interface IProjectRepository
{
    Task<List<Entities.Project>> RetrieveFullProjects();

    Task<Entities.Project> RetrieveProjectById(Guid projectId);

    Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId);

    Task<Requirement> RetrieveRequirementById(Guid requirementId);
}
