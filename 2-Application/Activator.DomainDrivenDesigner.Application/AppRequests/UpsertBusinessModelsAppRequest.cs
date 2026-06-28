using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record UpsertBusinessModelsAppRequest(Guid Id, Guid RequirementId, BusinessModel Model) : AppRequest(Id);
