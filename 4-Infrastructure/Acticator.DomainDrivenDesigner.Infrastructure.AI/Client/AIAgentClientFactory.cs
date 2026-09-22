using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OllamaSharp;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Client;

[ServiceLocate(default, ServiceType.Singleton)]
public class AIAgentClientFactory
{
    private readonly AIOptions _aiOption;
    private readonly ILogger _logger;

    public OllamaApiClient? OllamaApiClient { get; private set; }

    public AIAgentClientFactory(IOptions<AIOptions> aiOptions, ILogger logger)
    {
        _aiOption = aiOptions.Value;
        _logger = logger;
    }

    public AIAgent Get(string instructions)
    {
        return Create(_aiOption, instructions, false);
    }

    public AIAgent Get(string instructions, string model, bool applyQwenToolFix = false, IList<AITool>? tools = null)
    {
        var options = new AIOptions
        {
            Endpoint = _aiOption.Endpoint,
            Model = model
        };

        return Create(options, instructions, applyQwenToolFix, tools: tools);
    }

    private AIAgent Create(AIOptions opt, string instructions, bool applyQwenToolFix, IList<AITool>? tools = null)
    {
        // --- Agent Setup ---
        var ollamaApiClient = new OllamaApiClient(
            new Uri(opt.Endpoint),
            defaultModel: opt.Model
        );
        var chatClient = applyQwenToolFix == true
                        ? new ChatClientBuilder(ollamaApiClient)                                
                                .UseFunctionInvocation(loggerFactory: null, options => { options.MaximumIterationsPerRequest = 10; })
                                .Use(client => new ToolCallingFixMiddleware(client, _logger))
                                .Build()
                        : new ChatClientBuilder(ollamaApiClient)
                                .Build();

        var chatClientAgentOptions = new ChatClientAgentOptions
        {
            ChatOptions = new ChatOptions
            {
                Temperature = 0.0f,
                Seed = 42,
                Instructions = instructions,
                Tools = tools,
            }
        };

        OllamaApiClient = ollamaApiClient;

        return chatClient.AsAIAgent(
            chatClientAgentOptions
        );
    }
}