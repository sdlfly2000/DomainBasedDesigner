namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveRequirementByProjectAppRequest(Guid RequestId, Guid ProjectId) : AppRequest(RequestId);
