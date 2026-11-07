using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelsByNameAppResponse(Guid RequestId, bool Success, string? ErrorMessage, BusinessModel? BusinessModel)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public RetrieveBusinessModelsByNameAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
