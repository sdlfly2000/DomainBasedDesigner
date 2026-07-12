using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveRequirementResponse(Guid RequestId, Requirement? Requirement, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
