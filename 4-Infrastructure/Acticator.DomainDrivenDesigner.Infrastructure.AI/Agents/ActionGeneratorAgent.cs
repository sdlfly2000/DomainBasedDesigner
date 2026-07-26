using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Model;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class ActionGeneratorAgent
{
    private const string Instructions =
    """
    You are a expert of C# programming language. You can generate C# class files based on step-by-step instructions.
    
    Note:
    - Please follow output format.
    - Please make sure the generated C# class files are valid and can be compiled successfully.              
    """;

    private readonly AIAgent _aiAgent;

    public ActionGeneratorAgent(AIAgentClientFactory agentFactory, string model = "qwen2.5-coder:7b-instruct")
    {
        _aiAgent = agentFactory.Get(Instructions, model);
    }

    public async Task<AgentResponse<ActionGeneratorResult>> Create(string input, CancellationToken token)
    {
        return await _aiAgent
            .RunAsync<ActionGeneratorResult>(
            $"Please create following instruction to generate C# classes in C# syntax, {input}", 
            cancellationToken: token)
            .ConfigureAwait(false);
    }
}
