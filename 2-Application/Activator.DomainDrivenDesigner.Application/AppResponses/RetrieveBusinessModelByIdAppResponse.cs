using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelByIdAppResponse(Guid RequestId, BusinessModel? BusinessModel, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
