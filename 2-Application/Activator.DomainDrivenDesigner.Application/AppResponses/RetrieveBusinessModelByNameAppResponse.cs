using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelByNameAppResponse(Guid RequestId, bool Success, string? ErrorMessage, BusinessModel? BusinessModel)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public RetrieveBusinessModelByNameAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
