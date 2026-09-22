namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IDDDRepository
{
    Task<Guid?> CreateBusinessModel(BusinessModel.Entities.BusinessModel model, Guid requirementId);

    Task<List<BusinessModel.Entities.BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid RequirementId);
}
