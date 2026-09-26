namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record UpdateContextAppRequest(Guid Id, Guid ContextId, string Name) : AppRequest(Id);
