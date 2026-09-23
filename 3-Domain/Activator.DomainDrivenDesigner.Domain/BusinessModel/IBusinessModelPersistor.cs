namespace Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

public interface IBusinessModelPersistor
{
    Task<Guid> UpdateBusinessModel(BusinessModel model);

    Task<Guid> CreateBusinessModel(BusinessModel model, Guid requirementId);
}
