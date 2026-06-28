namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveBusinessModelsByNameAppRequest(Guid Id, Guid RequirementId, string ModelName) : AppRequest(Id);
