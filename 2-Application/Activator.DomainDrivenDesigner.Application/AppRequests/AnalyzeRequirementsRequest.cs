namespace Activator.DomainDrivenDesigner.Application.AppRequests;

public record AnalyzeRequirementsRequest(Guid RequestId, string RequirementDescription) : AppRequest(RequestId);
