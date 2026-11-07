namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveBusinessModelsByProjectAppRequest(Guid Id, Guid ProjectId) : AppRequest(Id);
