using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelsByProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage, List<BusinessModel>? BusinessModels)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public RetrieveBusinessModelsByProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
    }
};
