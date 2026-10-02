using Activator.DomainDrivenDesigner.Domain.BusinessModel.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveBusinessModelsAppResponse(Guid RequestId, List<BusinessModel>? BusinessModels, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
