using Activator.DomainDrivenDesigner.Support.Core.Configurations;
using Common.Core.AOP.LogTrace;
using Common.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Activator.DomainDrivenDesigner.Support.Core.Middleware;

[ServiceLocate(null)]
public class RequestArrivalMiddleware(
    IRequestContext requestContext)
    : IMiddleware
{
    async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
    {
        requestContext.TraceId = Guid.NewGuid().ToString();
        var appName = ConfigurationService.GetConfiguration()["Application:Properties:Name"];

        var currentUserId = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        var roles = context.User.Claims
            .Where(c => c.Type.Equals(ClaimTypes.Role))
            .Select(c => c.Value)
            .ToList();
        requestContext.CurrentUserId = string.IsNullOrEmpty(currentUserId) ? Guid.Empty : Guid.Parse(currentUserId);
        requestContext.CurrentUserRoles = GetRoleFromClaims(appName, roles);

        context.Response.Headers.Append("X-DDD-Trace-Id", requestContext.TraceId);

        await next.Invoke(context);
    }

    private List<string> GetRoleFromClaims(string? appName, List<string> allRoles)
    {
        var roles = new List<string>();
        
        if (allRoles.Count == 0 || string.IsNullOrEmpty(appName))
        {
            return roles;
        }

        allRoles.ForEach(role =>
        {
            var rolePair = role.Split(':');
            if(rolePair[0].Equals(appName, StringComparison.InvariantCultureIgnoreCase))
                roles.Add(rolePair[1]);
        });

        return roles;
    }
}
