namespace Activator.DomainDrivenDesigner.Domain.BusinessModel;

public interface IBusinessModelRepository
{
    Task<Entities.BusinessModel> RetrieveBusinessModelById(Guid businessModelId);

    Task<List<BusinessModel.Entities.BusinessModel>> RetrieveBusinessModelsByRequirementId(Guid RequirementId);
}
