using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveContextAppResponse(Guid RequestId, List<Context>? Contexts, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
