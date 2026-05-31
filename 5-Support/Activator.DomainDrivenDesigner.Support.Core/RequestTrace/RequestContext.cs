using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;

namespace Activator.DomainDrivenDesigner.Support.Core.RequestTrace;

[ServiceLocate(typeof(IRequestContext), ServiceType.Scoped)]
public class RequestContext : IRequestContext
{
    public string TraceId { get; set; }
    public Guid CurrentUserId { get; set; }
    public List<string> CurrentUserRoles { get; set; }
}
