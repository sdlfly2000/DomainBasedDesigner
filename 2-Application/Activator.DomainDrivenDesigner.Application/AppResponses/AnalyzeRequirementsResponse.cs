using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record AnalyzeRequirementsResponse(
    Guid RequestId,
    bool Success,
    string? ErrorMessage,
    BusinessModel[] BusinessModels,
    string raw)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public AnalyzeRequirementsResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, Array.Empty<BusinessModel>(), string.Empty)
    {
    }
};
