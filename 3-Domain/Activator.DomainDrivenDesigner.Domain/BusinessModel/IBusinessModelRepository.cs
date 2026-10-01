namespace Activator.DomainDrivenDesigner.Domain.BusinessModel;

public interface IBusinessModelRepository
{
    Task<Entities.BusinessModel> LoadBusinessModelById(Guid businessModelId);

    Task<List<BusinessModel.Entities.BusinessModel>> LoadBusinessModelsByRequirementId(Guid RequirementId);
}
