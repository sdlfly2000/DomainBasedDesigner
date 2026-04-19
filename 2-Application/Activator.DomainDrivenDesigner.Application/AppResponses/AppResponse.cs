namespace Activator.DomainDrivenDesigner.Application.AppResponses;

public abstract record AppResponse(Guid RequestId, bool Success, string? ErrorMessage);
