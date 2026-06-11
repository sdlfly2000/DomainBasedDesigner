using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;

namespace Acticator.DomainDrivenDesigner.Infrastructure.AI.Client;

[ServiceLocate(default, ServiceType.Singleton)]
public class AIAgentClientFactory
{
    private readonly AIOptions _aiOption;
    public AIAgentClientFactory(IOptions<AIOptions> aiOptions)
    {
        _aiOption = aiOptions.Value;
    }

    public AIAgent Get(string instructions)
    {
        return Create(_aiOption, instructions);
    }

    private AIAgent Create(AIOptions opt, string instructions)
    {
        // --- Agent Setup ---
        var ollamaApiClient = new OllamaApiClient(
            new Uri(opt.Endpoint),
            defaultModel: opt.Model
        );

        return new ChatClientAgent(
            ollamaApiClient,
            instructions: instructions
            );
    }
}
