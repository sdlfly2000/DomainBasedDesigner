using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Domain.Repositories;

public interface IBusinessModelRepository
{
    Task<BusinessModel> RetrieveBusinessModelsById(Guid businessModelId);

    Task<Guid> UpdateBusinessModels(BusinessModel model);
}
