using Acticator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Acticator.DomainDrivenDesigner.Infrastructure.AI;

[ServiceLocate(default)]
public class AIService
{
    private readonly AIAgent _agent;

    public AIService(AIAgentClientFactory agentFactory)
    {
        _agent = agentFactory.Get();    
    }


}
