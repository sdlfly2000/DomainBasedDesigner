using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Acticator.DomainDrivenDesigner.Infrastructure.AI.Client;

[ServiceLocate(default, ServiceType.Singleton)]
internal class AIAgentClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public AIAgentClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AIAgent Create(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}
