using Activator.DomainDrivenDesigner.Domain.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveFullProjectAppResponse(Guid RequestId, List<Project>? Projects, bool Success, string? ErrorMessage) 
    : AppResponse(RequestId, Success, ErrorMessage);
