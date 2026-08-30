namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record RetrieveContextAppRequest(Guid Id) : AppRequest(Id);
