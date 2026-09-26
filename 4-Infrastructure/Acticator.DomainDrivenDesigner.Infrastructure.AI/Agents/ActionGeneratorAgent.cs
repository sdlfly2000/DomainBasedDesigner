using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Markdig;
using Markdig.Syntax;
using Microsoft.Agents.AI;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.AI;
using OllamaSharp;
using Serilog;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;

public class ActionGeneratorAgent
{
    private string ReferenceArgumentPattern = @"(?<tool>\w+)\(""(?<path>[^""]+)""\)";
    private string ActionArgumentPattern    = @"(?<tool>\w+)\(""(?<path>[^""]+)"",""(?<method>[^""]+)""\)";
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
            var matchReference = Regex.Match(lines[i], ReferenceArgumentPattern);
            if (matchReference.Success && matchReference.Groups["tool"].Value == "read_code_file")
            {
                var argument = matchReference.Groups["path"].Value;
                var fileContent = read_code_file(argument);
                lines[i] = fileContent;
            }

            var matchAction = Regex.Match(lines[i], ActionArgumentPattern);
            if (matchAction.Success && matchAction.Groups["tool"].Value == "read_action_md_file")
            {
                var argument = matchAction.Groups["path"].Value;
                var method = matchAction.Groups["method"].Value;

                var fileContent = read_action_md_file(argument, method);
                lines[i] = fileContent;
            }
        }
        return string.Join(Environment.NewLine, lines);
    }

    [Description("Reads the content of an action in markdown file and returns it wrapped in a code block.")]
    private string read_action_md_file(string relativePath, string method)
    {
        var fileContent = ReadFileContent(relativePath);

        var actionDocument = Markdown.Parse(fileContent);

        var mermaidFencedCodeBlock = actionDocument.Descendants<FencedCodeBlock>()
                                             .SingleOrDefault(b =>
                                                    b.Info != null && b.Info.Equals("mermaid") &&
                                                    b.Arguments != null && b.Arguments.Contains(method));
                                
        if (mermaidFencedCodeBlock == null)
        {
            _logger.Warning($"{nameof(read_action_md_file)}: Warning: No single mermaid code block(s) found for method '{method}' in {relativePath}.");
            return string.Empty;
        }

        var mermaidFencedCodeBlockContent = string.Join(Environment.NewLine, mermaidFencedCodeBlock.Lines.Lines.Select(l => l.ToString()));

        var content = new StringBuilder();
        content.Append("```mermaid")
               .Append(Environment.NewLine)
               .Append(mermaidFencedCodeBlockContent)
               .Append(Environment.NewLine)
               .Append("```");

        return content.ToString();
    }

    [Description("Reads the content of a C# reference code file and returns it wrapped in a code block.")]
    private string read_code_file(string relativePath)
    {
        var fileContent = ReadFileContent(relativePath);

        var content = new StringBuilder();
        content.Append("```csharp")
               .Append(Environment.NewLine)
               .Append(fileContent)
               .Append(Environment.NewLine)
               .Append("```");

        return content.ToString();
    }

    private string ReadFileContent(string relativePath)
    {
        string fullPath = Path.GetFullPath(Path.Combine(_projectBaseDirectory, relativePath));
        if (!fullPath.StartsWith(_projectBaseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            _logger.Warning($"{nameof(ReadFileContent)}: Warning: Access denied. Cannot read files outside the workspace root. {relativePath}");
            return string.Empty;
        }

        if (!File.Exists(fullPath))
        {
            _logger.Warning($"{nameof(ReadFileContent)}: Warning: File not found at path '{relativePath}'.");
            return string.Empty;
        }

        _logger.Information($"{nameof(ReadFileContent)}: Reading file at path '{relativePath}'.");
        return File.ReadAllText(fullPath);
    }
}