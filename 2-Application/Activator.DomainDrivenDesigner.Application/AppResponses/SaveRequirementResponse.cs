namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record SaveRequirementResponse(
    Guid RequestId,
    bool Success, 
    string? ErrorMessage)
    : AppResponse(RequestId, Success, ErrorMessage);
