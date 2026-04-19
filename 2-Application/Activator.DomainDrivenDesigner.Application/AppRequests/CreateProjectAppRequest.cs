namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record CreateProjectAppRequest(Guid Id, string ProjectName) : AppRequest(Id);
