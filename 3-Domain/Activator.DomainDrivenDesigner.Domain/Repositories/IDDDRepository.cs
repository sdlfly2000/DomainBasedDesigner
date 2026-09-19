using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IDDDRepository
{
    Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId);

    Task<Guid?> UpdateRequirement(Requirement requirement);

    Task<Guid?> CreateBusinessModel(BusinessModel model, Guid requirementId);

    Task<Project.Entities.Project> RetrieveProjectById(Guid projectId);

    Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId);

    Task<Requirement> RetrieveRequirementById(Guid requirementId);

    Task<List<BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid RequirementId);
}
