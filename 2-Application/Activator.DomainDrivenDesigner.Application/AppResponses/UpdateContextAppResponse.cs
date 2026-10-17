namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record UpdateContextAppResponse(Guid RequestId, bool Success, string? ErrorMessage, Guid? ContextId)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public UpdateContextAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
