using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementByProjectAppResponse(Guid RequestId, List<Requirement>? Requirements, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
