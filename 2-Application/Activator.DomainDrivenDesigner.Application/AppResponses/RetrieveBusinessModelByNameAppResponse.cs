using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelByNameAppResponse(Guid RequestId, BusinessModel? BusinessModel, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
