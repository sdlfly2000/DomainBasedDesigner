using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;

namespace Acticator.DomainDrivenDesigner.Infrastructure.AI.Client;

[ServiceLocate(default, ServiceType.Singleton)]
public class AIAgentClientFactory
{
    private Lazy<AIAgent> _aiAgent;

    public AIAgentClientFactory(IOptions<AIOptions> aiOptions)
    {
        _aiAgent = new Lazy<AIAgent>(() => Create(aiOptions.Value), isThreadSafe: true);
    }

    public AIAgent Get()
    {
        return _aiAgent.Value;
    }

    private AIAgent Create(AIOptions opt)
    {
        // --- Agent Setup ---
        var ollamaApiClient = new OllamaApiClient(
            new Uri(opt.Endpoint),
            defaultModel: opt.Model
        );

        return new ChatClientAgent(ollamaApiClient);
    }
}
