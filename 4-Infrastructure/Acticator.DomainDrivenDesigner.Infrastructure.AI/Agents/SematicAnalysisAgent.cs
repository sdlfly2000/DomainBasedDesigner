using Acticator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class SematicAnalysisAgent
{
    private const string Instructions = 
    """
    You are a expert of Mermaid Markdown. From now on, all code you create is for Mermaid Chart.
    
    terms:
    Entity == Class
    Attribute == Property
    """;

    private readonly AIAgent _aiAgent;

    public SematicAnalysisAgent(AIAgentClientFactory agentFactory)
    {
        _aiAgent = agentFactory.Get(Instructions);
    }

    public async Task<AgentResponse> Analyze(string input)
    {
        return await _aiAgent.RunAsync(input);
    }
}
