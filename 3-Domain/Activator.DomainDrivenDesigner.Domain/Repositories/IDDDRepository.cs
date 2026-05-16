using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IDDDRepository
{
    Task<Guid?> CreateProject(Project project);

    Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId);

    Task<List<Project>> RetrieveFullProjects();

    Task<List<BusinessModel>> RetrieveBusinessModelsByProjectId(Guid ProjectId);

    Task<List<BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid RequirementId);

    Task<BusinessModel> RetrieveBusinessModelsById(Guid BusinessModelId);
}
