using Activator.DomainDrivenDesigner.Domain.Project.Entities;

namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record RetrieveFullProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage, List<Project>? Projects)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public RetrieveFullProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
