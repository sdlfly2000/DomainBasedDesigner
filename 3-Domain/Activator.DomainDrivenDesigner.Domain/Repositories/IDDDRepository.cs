using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IDDDRepository
{  
    Task<Guid?> UpdateRequirement(Requirement requirement);

    Task<Guid?> CreateBusinessModel(BusinessModel.Entities.BusinessModel model, Guid requirementId);

    Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId);

    Task<Requirement> RetrieveRequirementById(Guid requirementId);

    Task<List<BusinessModel.Entities.BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid RequirementId);
}
