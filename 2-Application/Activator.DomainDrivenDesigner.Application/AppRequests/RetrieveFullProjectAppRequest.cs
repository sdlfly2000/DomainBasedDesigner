namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveFullProjectAppRequest(Guid Id) : AppRequest(Id);
