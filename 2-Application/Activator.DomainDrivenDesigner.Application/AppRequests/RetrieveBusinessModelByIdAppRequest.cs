namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveBusinessModelByIdAppRequest(Guid RequestId, Guid ModelId) : AppRequest(RequestId);
