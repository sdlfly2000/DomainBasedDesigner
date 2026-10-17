using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelsAppResponse(Guid RequestId, bool Success, string? ErrorMessage, List<BusinessModel>? BusinessModels)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public RetrieveBusinessModelsAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
