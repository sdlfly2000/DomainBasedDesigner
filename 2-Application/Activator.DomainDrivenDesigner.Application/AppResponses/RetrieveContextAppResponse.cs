using Activator.DomainDrivenDesigner.Domain.Context.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveContextAppResponse(Guid RequestId, bool Success, string? ErrorMessage, List<Context>? Contexts) 
    : AppResponse(RequestId, Success, ErrorMessage);
