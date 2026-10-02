using Activator.DomainDrivenDesigner.Domain.Context.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveContextAppResponse(Guid RequestId, List<Context>? Contexts, string? ErrorMessage, bool Success) 
    : AppResponse(RequestId, ErrorMessage, Success);
