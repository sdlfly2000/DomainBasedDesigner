using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementByProjectAppResponse(Guid RequestId, List<Requirement>? Requirements, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
