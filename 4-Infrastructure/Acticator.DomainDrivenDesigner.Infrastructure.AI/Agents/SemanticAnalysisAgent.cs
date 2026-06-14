using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Model;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class SemanticAnalysisAgent
{
    private const string Instructions =
    """
    You are a language analysis expert who looks for verbs, nouns, and modifiers in sentences. 
    
    You will also identify the relationships between these components, such as relationships between modifiers and nouns.
    
    Your task is to analyze the given sentences and extract this information in a structured format.

    Example:

    Input: "The quick brown fox jumps over the lazy dog."

    Output:
    {
        "verbs": ["jumps"],
        "nouns": ["fox", "dog"],
        "modifiers": ["quick", "brown", "lazy"],
        "relationships": [["fox",["quick", "brown"]],["dog",["lazy"]]]
    }

    """;

    private readonly AIAgent _aiAgent;

    public SemanticAnalysisAgent(AIAgentClientFactory agentFactory)
    {
        _aiAgent = agentFactory.Get(Instructions);
    }

    public async Task<AgentResponse<SemanticAnalysisResult>> Analyze(string input)
    {
        return await _aiAgent.RunAsync<SemanticAnalysisResult>($"please analyze sentence: {input}");
    }
}
