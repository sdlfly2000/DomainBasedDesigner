using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Model;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class ModelGeneratorAgent
{
    private const string Instructions =
    """
    You are a expert of C# programming language. You can convert Mermaid Class Diagram to C# class file.
    
    Note:
    - Please follow output format.
    - Please make sure the generated C# class files are valid and can be compiled successfully.

    Example

    Mermaid Class Diagram:
    ClassDiagram
        %% Domain/User/User.cs
        class User {
            +Guid Id
            +String Name
            +String Email
        }

        %% Application/Model/Product.cs
        class Product {
            +Guid Id
            +String Name
            +Decimal Price
        }

    C# class files:
    // Domain/User/User.cs
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // Application/Model/Product.cs
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    Output format:
    [
        {"file_path": "Domain//User//User.cs", "content": "public class User \n { public Guid Id { get; set; } \n public string Name { get; set; } \n public string Email { get; set; } \n }"},
        {"file_path": "Application//Model//Product.cs", "content": "public class Product \n { public Guid Id { get; set; } \n public string Name { get; set; } \n public decimal Price { get; set; } \n }"}
    ]
              
    """;

    private readonly AIAgent _aiAgent;

    public ModelGeneratorAgent(AIAgentClientFactory agentFactory, string model = "qwen2.5-coder:7b-instruct")
    {
        _aiAgent = agentFactory.Get(Instructions, model);
    }

    public async Task<AgentResponse<ModelGeneratorResult[]>> Create(string input, CancellationToken token)
    {
        return await _aiAgent
            .RunAsync<ModelGeneratorResult[]>(
            $"Please create following Mermaid Class Diagram to C# classes in C# syntax, {input}", 
            cancellationToken: token)
            .ConfigureAwait(false);
    }
}
