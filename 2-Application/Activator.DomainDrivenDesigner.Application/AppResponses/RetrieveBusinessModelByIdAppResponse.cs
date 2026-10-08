using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelByIdAppResponse : AppResponse
{
    public RetrieveBusinessModelByIdAppResponse(Guid RequestId, bool Success, string? ErrorMessage, BusinessModel? BusinessModel): base(RequestId, Success, ErrorMessage)
    {
        Model = BusinessModel;
    }

    public RetrieveBusinessModelByIdAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : base(RequestId, Success, ErrorMessage)
    {
        
    }

    public BusinessModel? Model;
};
