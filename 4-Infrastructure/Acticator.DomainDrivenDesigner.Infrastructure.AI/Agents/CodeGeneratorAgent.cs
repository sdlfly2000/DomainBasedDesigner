using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Helper;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using Serilog;
using System.Text.RegularExpressions;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

public class CodeGeneratorAgent
{
    private string ReferenceArgumentPattern = @"(?<tool>\w+)\(""(?<path>[^""]+)""\)";
    private string ActionArgumentPattern = @"(?<tool>\w+)\(""(?<path>[^""]+)"",""(?<method>[^""]+)""\)";
    private string ModelArgumentPattern = @"(?<tool>\w+)\(""(?<path>[^""]+)"",""(?<namespace>[^""]+)"",""(?<model>[^""]+)""\)";
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
    private readonly FileProcessHelper _fileProcessHelper;

    public CodeGeneratorAgent(ILogger logger, AIAgentClientFactory agentFactory, string projectBaseDirectory, string model = "qwen2.5-coder:7b-instruct", bool applyQwenToolFix = false)
    {
        _projectBaseDirectory = projectBaseDirectory;
        _model = model;
        _aiAgentClientFactory = agentFactory;
        _logger = logger;
        _fileProcessHelper = new FileProcessHelper(_logger, _projectBaseDirectory);
        _aiAgent = applyQwenToolFix
                    ? agentFactory.Get(ToolCallInstructions, model, applyQwenToolFix, [AIFunctionFactory.Create(_fileProcessHelper.read_code_file, "read_code_file")])
                    : agentFactory.Get(Instructions, model, applyQwenToolFix);

    }

    public async Task<string> Create(string input, CancellationToken token)
    {
        var parsedInput = FindAndReplaceReference(input);

        _logger.Debug($"Parsed input: {Environment.NewLine}{parsedInput}");

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
            var matchReference = Regex.Match(lines[i], ReferenceArgumentPattern);
            if (matchReference.Success && matchReference.Groups["tool"].Value == "read_code_file")
            {
                var argument = matchReference.Groups["path"].Value;
                var fileContent = _fileProcessHelper.read_code_file(argument);
                lines[i] = fileContent;
            }

            var matchAction = Regex.Match(lines[i], ActionArgumentPattern);
            if (matchAction.Success && matchAction.Groups["tool"].Value == "read_action_md_file")
            {
                var argument = matchAction.Groups["path"].Value;
                var method = matchAction.Groups["method"].Value;

                var fileContent = _fileProcessHelper.read_action_md_file(argument, method);
                lines[i] = fileContent;
            }

            var matchModel = Regex.Match(lines[i], ModelArgumentPattern);
            if (matchModel.Success && matchModel.Groups["tool"].Value == "read_model_md_file")
            {
                var argument = matchModel.Groups["path"].Value;
                var ns = matchModel.Groups["namespace"].Value;
                var model = matchModel.Groups["model"].Value;

                var fileContent = _fileProcessHelper.read_model_md_file(argument, ns, model);
                lines[i] = fileContent;
            }
        }
        return string.Join(Environment.NewLine, lines); 
    }
}