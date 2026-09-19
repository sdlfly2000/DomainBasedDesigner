using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record AnalyzeRequirementsResponse(
    Guid RequestId,
    BusinessModel[] BusinessModels,
    string raw,
    bool Success, 
    string? ErrorMessage)
    : AppResponse(RequestId, Success, ErrorMessage);
