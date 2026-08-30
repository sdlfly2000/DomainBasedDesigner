namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record CreateContextAppResponse(Guid RequestId, Guid? ContextId, bool Success, string? ErrorMessage)
    : AppResponse(RequestId, Success, ErrorMessage);
