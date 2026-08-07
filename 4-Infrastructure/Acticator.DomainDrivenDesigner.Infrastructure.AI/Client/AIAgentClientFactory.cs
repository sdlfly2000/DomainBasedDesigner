using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Client;

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

    public AIAgent Get(string instructions, string model)
    {
        var options = new AIOptions
        {
            Endpoint = _aiOption.Endpoint,
            Model = model
        };

        return Create(options, instructions);
    }

    private AIAgent Create(AIOptions opt, string instructions)
    {
        // --- Agent Setup ---
        var ollamaApiClient = new OllamaApiClient(
            new Uri(opt.Endpoint),
            defaultModel: opt.Model
        );

        var chatClientAgentOptions = new ChatClientAgentOptions
        {
            ChatOptions = new Microsoft.Extensions.AI.ChatOptions
            {
                Temperature = 0.0f,
                Instructions = instructions
            }
        };

        return new ChatClientAgent(
            ollamaApiClient,
            chatClientAgentOptions);
    }
}
