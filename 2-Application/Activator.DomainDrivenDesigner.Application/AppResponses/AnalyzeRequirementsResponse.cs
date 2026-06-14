namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record AnalyzeRequirementsResponse(Guid RequestId, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
