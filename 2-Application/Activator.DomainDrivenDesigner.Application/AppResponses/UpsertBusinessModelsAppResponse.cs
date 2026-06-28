namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record UpsertBusinessModelsAppResponse(Guid RequestId, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
