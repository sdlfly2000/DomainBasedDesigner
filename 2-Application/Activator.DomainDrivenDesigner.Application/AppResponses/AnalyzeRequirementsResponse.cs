using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record AnalyzeRequirementsResponse(
    Guid RequestId,
    bool Success, 
    string? ErrorMessage,
    BusinessModel[] BusinessModels,
    string raw)
    : AppResponse(RequestId, Success, ErrorMessage);
