namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveContextAppRequest(Guid Id, Guid ProjectId) : AppRequest(Id);
