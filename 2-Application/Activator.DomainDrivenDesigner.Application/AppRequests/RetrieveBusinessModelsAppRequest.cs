namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveBusinessModelsAppRequest(Guid Id, Guid ProjectId) : AppRequest(Id);
