namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record CreateProjectAppRequest(Guid Id, string ProjectName, string ProjectDescription, string? ProjectBaseDirectory = null) : AppRequest(Id);
