using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelsAppResponse(Guid RequestId, List<BusinessModel>? BusinessModels, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
