using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementResponse(Guid RequestId, Requirement? Requirement, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
