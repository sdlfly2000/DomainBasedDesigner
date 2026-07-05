namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record SaveRequirementRequest(Guid RequestId, Guid ProjectId, Guid? RequirementId, string RequirementDescription) : AppRequest(RequestId);
