using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Support.Core.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Serilog;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class CodeGeneratorAgentTest
{
    private CodeGeneratorAgent _actionGeneratorAgent;
    private string _projectBaseDirectory;
    private ILogger _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            //.MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions, _logger);

        _projectBaseDirectory = "C:\\Users\\25982\\Documents\\Projects\\DomainBasedDesigner";

        //_actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory, "ornith:9b", false);
        _actionGeneratorAgent = new CodeGeneratorAgent(_logger, aIAgentClientFactory, _projectBaseDirectory, applyQwenToolFix: false);
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidInstructionIsProvided()
    {
        // Arrange
        var aiInstruction = Path.Combine(_projectBaseDirectory, "AI", "Models", "BusinessModel.md");
        var instruction = await File.ReadAllTextAsync(aiInstruction);

        // Action
        var result = await _actionGeneratorAgent.Create(instruction, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        _logger.Information(result);
        //Console.WriteLine(string.Concat("File: ", result.Result.file_path));
        //Console.WriteLine(string.Concat("Content: ", Environment.NewLine, result.Result.content));
    }

    [TearDown]
    public void CleanUp()
    {
        if (_logger is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}