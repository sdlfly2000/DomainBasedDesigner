namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record SaveRequirementRequest(Guid RequestId, string RequirementDescription) : AppRequest(RequestId);
