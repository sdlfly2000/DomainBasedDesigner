namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record UpdateContextAppResponse(Guid RequestId, Guid? ContextId, bool Success, string? ErrorMessage)
    : AppResponse(RequestId, Success, ErrorMessage);
