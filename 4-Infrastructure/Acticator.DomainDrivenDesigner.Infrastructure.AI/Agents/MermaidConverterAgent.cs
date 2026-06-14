using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class MermaidConverterAgent
{
    private const string Instructions =
    """
    You are a expert of Mermaid Markdown. You can convert structured class information to Mermaid Class Chart.

    Note: Do not include ```mermaid and ```, just return the content between them.

    Example

    structured class:
    
    [
        {
            "name": "User",
            "properties": [
                {
                    "Name": "Id",
                    "Type": "Guid"
                },
                {
                    "Name": "Name",
                    "Type": "String"
                },
                {
                    "Name": "Email",
                    "Type": "String"
                },
                {
                    "Name": "new",
                    "Type": null
                }
            ],
            "methods": ["Login()", "Logout()"]
        },
        {
            "name": "Product",
            "properties": [            
                {
                    "Name": "Id",
                    "Type": "Guid"
                },
                {
                    "Name": "Name",
                    "Type": "String"
                },
                {
                    "Name": "Price",
                    "Type": "Decimal"
                }
            ]
        }
    ]
    

    Mermaid Class Chart

    classDiagram
        class User {
            + Id: Guid
            + Name: String
            + Email: String
            + new
            + Login(): boolean
            + Logout(): void
        }

        class Product {
            + Id: Guid
            + Name: String
            + Price: Decimal
        }
    """;

    private readonly AIAgent _aiAgent;

    public MermaidConverterAgent(AIAgentClientFactory agentFactory)
    {
        _aiAgent = agentFactory.Get(Instructions);
    }

    public async Task<AgentResponse> Convert(string input)
    {
        return await _aiAgent.RunAsync($"Please convert: {input}");
    }
}
