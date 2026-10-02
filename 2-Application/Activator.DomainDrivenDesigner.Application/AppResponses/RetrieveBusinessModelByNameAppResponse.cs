using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelByNameAppResponse(Guid RequestId, BusinessModel? BusinessModel, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
