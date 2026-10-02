namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record UpdateContextAppResponse(Guid RequestId, Guid? ContextId, string? ErrorMessage, bool Success)
    : AppResponse(RequestId, ErrorMessage, Success);
