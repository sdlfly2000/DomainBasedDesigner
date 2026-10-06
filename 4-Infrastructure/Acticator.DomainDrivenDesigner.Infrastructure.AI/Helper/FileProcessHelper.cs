using Markdig;
using Markdig.Syntax;
using Serilog;
using System.ComponentModel;
using System.Text;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Helper
{
    internal class FileProcessHelper
    {
        private string _projectBaseDirectory;
        private readonly ILogger _logger;

        internal FileProcessHelper(ILogger logger, string projectBaseDirectory)
        {
            _logger = logger;
            _projectBaseDirectory = projectBaseDirectory;
        }

        [Description("Reads the content of an model in markdown file and returns it wrapped in a code block.")]
        internal string read_model_md_file(string relativePath, string ns, string model)
        {
            var fileContent = ReadFileContent(relativePath, string.Concat(ns, "->", model));

            var pipeline = new MarkdownPipelineBuilder().UseDiagrams().Build();

            var modelDocument = Markdown.Parse(fileContent, pipeline);

            // Find the mermaid code block
            var mermaidFencedCodeBlock = modelDocument.Descendants<FencedCodeBlock>()
                                                      .SingleOrDefault(b =>
                                                        b.Info != null && b.Info.Equals("mermaid"));

            if (mermaidFencedCodeBlock == null)
            {
                _logger.Warning($"{nameof(read_model_md_file)}: Warning: No single mermaid code block(s) found for model '{ns}->{model}' in {relativePath}.");
                return string.Empty;
            }

            var findNamespace = false;
            var findModel = false;
            var parenthsis = new Stack<int>();
            var modelLines = new List<string>();

            foreach (var line in mermaidFencedCodeBlock.Lines.Lines)
            {
                var currentLine = line.ToString();

                // Find namespace in mermaid code block
                if (currentLine.Contains("namespace"))
                {
                    parenthsis.Push(1);

                    if (currentLine.Contains(ns))
                    {
                        findNamespace = true;
                    }
                }

                if (parenthsis.Count == 1)
                {
                    if (currentLine.Contains("class"))
                    {
                        parenthsis.Push(2);

                        if (findNamespace)
                        {
                            // Find model in namespace
                            if (currentLine.Contains($"class {model}"))
                            {
                                findModel = true;
                            }
                        }
                    }

                }

                if (findNamespace && findModel)
                {
                    modelLines.Add(currentLine);
                }

                if (currentLine.Trim().Equals("}"))
                {
                    var popValue = parenthsis.Pop();

                    if (popValue == 2 && findModel == true)
                    {
                        findModel = false;
                    }

                    if (popValue == 1 && findNamespace == true)
                    {
                        findNamespace = false;
                    }
                }
            }

            var modelContent = string.Join(Environment.NewLine, modelLines
                                                                .Where(l => !string.IsNullOrWhiteSpace(l.ToString()))
                                                                .ToList());

            var content = new StringBuilder();
            content.Append("```mermaid")
                   .Append(Environment.NewLine)
                   .Append(modelContent)
                   .Append(Environment.NewLine)
                   .Append("```");

            return content.ToString();
        }

        [Description("Reads the content of an action in markdown file and returns it wrapped in a code block.")]
        internal string read_action_md_file(string relativePath, string method)
        {
            var fileContent = ReadFileContent(relativePath, method);

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

            var mermaidFencedCodeBlockContent = string.Join(
                                                    Environment.NewLine,
                                                    mermaidFencedCodeBlock.Lines.Lines
                                                    .Where(l => !string.IsNullOrWhiteSpace(l.ToString()))
                                                    .Select(l => l.ToString()));

            var content = new StringBuilder();
            content.Append("```mermaid")
                   .Append(Environment.NewLine)
                   .Append(mermaidFencedCodeBlockContent)
                   .Append(Environment.NewLine)
                   .Append("```");

            return content.ToString();
        }

        [Description("Reads the content of a C# reference code file and returns it wrapped in a code block.")]
        internal string read_code_file(string relativePath)
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

        private string ReadFileContent(string relativePath, string? target = null)
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

            _logger.Information($"{nameof(ReadFileContent)}: Reading file at path '{relativePath}' {target ?? ""}.");
            return File.ReadAllText(fullPath);
        }
    }
}
