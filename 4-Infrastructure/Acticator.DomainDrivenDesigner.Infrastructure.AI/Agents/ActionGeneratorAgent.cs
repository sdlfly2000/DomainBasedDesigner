using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Common.Core.DependencyInjection;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using System.Text;
using System.Text.RegularExpressions;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

[ServiceLocate(default, ServiceType.Singleton)]
public class ActionGeneratorAgent
{
    private string ArgumentPattern = @"(?<tool>\w+)\(""(?<path>[^""]+)""\)";
    private string _projectBaseDirectory;
    private readonly ILogger _logger;

    private const string ToolCallInstructions =
    """
    You are a expert of C# programming language. You can generate C# class files based on step-by-step instructions.
        
    CRITICAL RULE: 
    Before generating any code file, you MUST inspect your dependency contracts.
    Do not generate any markdown or code block. Only output the JSON object.
    Do not generate code or assume model properties until you have requested and evaluated the file contents.
    """;

    private const string Instructions =
    """
    You are a expert of C# programming language. You can generate C# class files based on step-by-step instructions.
    """;

    private readonly string _model;
    private readonly AIAgent _aiAgent;
    private readonly AIAgentClientFactory _aiAgentClientFactory;

    public ActionGeneratorAgent(ILogger logger, AIAgentClientFactory agentFactory, string projectBaseDirectory, string model = "qwen2.5-coder:7b-instruct", bool applyQwenToolFix = false)
    {
        _projectBaseDirectory = projectBaseDirectory;
        _model = model;
        _aiAgentClientFactory = agentFactory;
        _logger = logger;
        _aiAgent = applyQwenToolFix 
                    ? agentFactory.Get(ToolCallInstructions, model, applyQwenToolFix, [AIFunctionFactory.Create(this.read_code_file, "read_code_file")])
                    : agentFactory.Get(Instructions, model, applyQwenToolFix);
    }

    public async Task<string> Create(string input, CancellationToken token)
    {
        var parsedInput = FindAndReplaceReference(input);
        var response = await _aiAgent
            .RunAsync(
            $"Please create following instruction to generate C# classes in C# syntax, {parsedInput}",
            cancellationToken: token)
            .ConfigureAwait(false);

        if (_aiAgentClientFactory != null && _aiAgentClientFactory.OllamaApiClient != null)
        {
            await _aiAgentClientFactory.OllamaApiClient.RequestModelUnloadAsync(_model).ConfigureAwait(false);
        }

        return response.Text.Replace("```csharp", "").Replace("```", "");
    }

    private string FindAndReplaceReference(string input)
    {
        string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < lines.Length; i++)
        {
            var match = Regex.Match(lines[i], ArgumentPattern);
            if (match.Success && match.Groups["tool"].Value == "read_code_file")
            {
                var argument = match.Groups["path"].Value;
                var fileContent = read_code_file(argument);
                lines[i] = fileContent;
            }
        }
        return string.Join(Environment.NewLine, lines);
    }

    private string read_code_file(string relativePath)
    {
        // Security boundary check
        string fullPath = Path.GetFullPath(Path.Combine(_projectBaseDirectory, relativePath));
        if (!fullPath.StartsWith(_projectBaseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning($"{nameof(read_code_file)}: Warning: Access denied. Cannot read files outside the workspace root. {relativePath}");
            return "Error: Access denied. Cannot read files outside the workspace root.";
        }

        if (!File.Exists(fullPath))
        {
            _logger.LogWarning($"{nameof(read_code_file)}: Warning: File not found at path '{relativePath}'.");
            return $"Error: File not found at path '{relativePath}'.";
        }

        _logger.LogInformation($"{nameof(read_code_file)}: Reading file at path '{relativePath}'.");
        var fileContent = File.ReadAllText(fullPath);
        var content = new StringBuilder();
        content.Append("```csharp")
               .Append(Environment.NewLine)
               .Append(fileContent)
               .Append(Environment.NewLine)
               .Append("```");

        return content.ToString();
    }
}