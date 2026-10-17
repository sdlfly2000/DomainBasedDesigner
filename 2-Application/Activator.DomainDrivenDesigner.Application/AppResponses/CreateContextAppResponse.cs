namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public record CreateContextAppResponse(Guid RequestId, bool Success, string? ErrorMessage, Guid? ContextId)
    : AppResponse(RequestId, Success, ErrorMessage)
{
    public CreateContextAppResponse(Guid RequestId, bool Success, string? ErrorMessage) : this(RequestId, Success, ErrorMessage, null)
    {
        
    }
};
