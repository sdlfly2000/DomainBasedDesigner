using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using Activator.DomainDrivenDesigner.Support.Core.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Serilog;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class ActionGeneratorAgentTest
{
    private ActionGeneratorAgent _actionGeneratorAgent;
    private ILogger _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions, _logger);

        var projectBaseDirectory = "C:\\Users\\25982\\Documents\\Projects\\DomainBasedDesigner";

        //_actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory, "ornith:9b", false);
        _actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory, projectBaseDirectory, applyQwenToolFix: false);
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidInstructionIsProvided()
    {
        // Arrange
        var instruction =
            """
            ## Generate complete C# code 
            Target File: **2-Application/Activator.DomainDrivenDesigner.Application/Services/ProjectAppService.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **ProjectAppService** class

                ```csharp
                namespace Activator.DomainDrivenDesigner.Application.Services;

                public class ProjectAppService
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **ProjectAppService**, Implements: **IProjectAppService**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Application.Services;`

            3. Inject below through constructor
            - **IProjectRepository** (store in a private readonly field `_projectRepository`)
            - **IProjectPersistor** (store in a private readonly field `_projectPersistor`)
            - **IServiceProvider** (store in a private readonly field `_serviceProvider`)

            4. Place Attributes
            - Decorate **ProjectAppService** class with [ServiceLocate(typeof(ProjectAppService))].
            - Decorate each public method with `[LogTrace(typeof({ResponseTypeName}))]`, replacing `{ResponseTypeName}` with the corresponding concrete return type. Ensure the `[LogTrace]` attribute target type references the underlying response record, *not* the wrapping `Task<>` type.

            5. Public async method signature: `Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)`

                Method **Create** logic: 
                Execute `read_action_md_file("UML//Application//ProjectAppService.md","Create")`.

            5. Public async method signature: `Task<RetrieveFullProjectAppResponse> RetrieveFullProjects(RetrieveFullProjectAppRequest request)`

                Method **RetrieveFullProjects** logic: 
                Execute `read_action_md_file("UML//Application//ProjectAppService.md","RetrieveFullProjects")`.

            ## Context Boundaries:
            - **Ignore Exception Handling:** Omit manual try-catch wrappers since exceptions are decoupled via the infrastructure `LogTrace` attribute tier.
            - **Asynchronous Execution:** Every data tier interaction must map via explicit asynchronous operations utilizing `ConfigureAwait(false)`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Project//Entities//Project.cs")`.

            ## Reference Dependency Interface Signatures:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Project//IProjectRepository.cs")`.
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Project//IProjectPersistor.cs")`.

            ## Reference Requests and Responses:
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//AppRequest.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//AppResponse.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//CreateProjectAppRequest.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//CreateProjectAppResponse.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//RetrieveFullProjectAppRequest.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//RetrieveFullProjectAppResponse.cs")`.

            ## Output
            Only output full source code of **ProjectAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.
            """;

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