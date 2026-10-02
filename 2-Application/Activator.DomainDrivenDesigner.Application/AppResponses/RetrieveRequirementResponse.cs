using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementResponse(Guid RequestId, bool Success, string? ErrorMessage, Requirement? Requirement) 
    : AppResponse(RequestId, Success, ErrorMessage);
