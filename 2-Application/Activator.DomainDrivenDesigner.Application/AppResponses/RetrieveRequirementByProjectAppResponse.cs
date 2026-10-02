using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementByProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage, List<Requirement>? Requirements) 
    : AppResponse(RequestId, Success, ErrorMessage);
