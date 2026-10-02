namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public abstract record AppResponse(Guid RequestId, string? ErrorMessage, bool Success);
