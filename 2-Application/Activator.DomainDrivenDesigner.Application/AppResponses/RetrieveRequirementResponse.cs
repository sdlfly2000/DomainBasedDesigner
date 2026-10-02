using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementResponse(Guid RequestId, Requirement? Requirement, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
