namespace Activator.DomainDrivenDesigner.Domain.BusinessModel;

public interface IBusinessModelRepository
{
    Task<Entities.BusinessModel> RetrieveBusinessModelsById(Guid businessModelId);

    Task<Guid> UpdateBusinessModels(Entities.BusinessModel model);
}
