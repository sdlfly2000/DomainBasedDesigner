using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record AnalyzeRequirementsResponse(
    Guid RequestId, 
    BusinessModel[]? BusinessModels, 
    BusinessAction[]? BusinessActions,
    string raw,
    bool Success, 
    string? ErrorMessage)
    : AppResponse(RequestId, Success, ErrorMessage);
