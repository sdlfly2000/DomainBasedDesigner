using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Model;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class ActionGeneratorAgent
{
    private const string Instructions =
    """
    You are a expert of C# programming language. You can generate C# class files based on step-by-step instructions.
        
    CRITICAL RULE: 
    Before generating any code file, you MUST inspect your dependency contracts.
    If you need to read a file, output a single, raw text block in this exact format:
    {"name": "read_code_file", "arguments": {"relativePath": "your/file/path.cs"}}
    Do not generate any markdown or code block. Only output the JSON object.
    Do not generate code or assume model properties until you have requested and evaluated the file contents.
    """;

    private readonly AIAgent _aiAgent;

    public ActionGeneratorAgent(AIAgentClientFactory agentFactory, string model = "qwen2.5-coder:7b-instruct")
    {
        _aiAgent = agentFactory.Get(Instructions, model, true, [AIFunctionFactory.Create(this.read_code_file, "read_code_file")]);
    }

    public async Task<AgentResponse<ActionGeneratorResult>> Create(string input, CancellationToken token)
    {
        return await _aiAgent
            .RunAsync<ActionGeneratorResult>(
            $"Please create following instruction to generate C# classes in C# syntax, {input}", 
            cancellationToken: token)
            .ConfigureAwait(false);
    }

    public async Task<AgentResponse> CreateDebug(string input, CancellationToken token)
    {
        return await _aiAgent
            .RunAsync(
            $"Please create following instruction to generate C# classes in C# syntax, {input}",
            cancellationToken: token)
            .ConfigureAwait(false);
    }

    [Description("read_code_file")]
    private string read_code_file(
        [Description("The relative path of the C# file (Example: 4-Infrastructure/T_PROJECT.cs)")]
        string relativePath)
    {
        var projectBaseDirectory = "C:\\Users\\25982\\Documents\\Projects\\DomainBasedDesigner";

        // Security boundary check
        string fullPath = Path.GetFullPath(Path.Combine(projectBaseDirectory, relativePath));
        if (!fullPath.StartsWith(projectBaseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return "Error: Access denied. Cannot read files outside the workspace root.";
        }

        if (!File.Exists(fullPath))
        {
            return $"Error: File not found at path '{relativePath}'.";
        }

        return File.ReadAllText(fullPath);
    }
}