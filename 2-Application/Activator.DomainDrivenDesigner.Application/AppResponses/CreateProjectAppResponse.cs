namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record CreateProjectAppResponse(Guid RequestId, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
