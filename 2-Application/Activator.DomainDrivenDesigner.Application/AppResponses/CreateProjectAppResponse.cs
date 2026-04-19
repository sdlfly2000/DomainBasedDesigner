namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record CreateProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
