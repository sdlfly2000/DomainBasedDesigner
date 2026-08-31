namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record CreateContextAppRequest(Guid Id, string Name, Guid ProjectId) : AppRequest(Id);
