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
            File: **2-Application/Activator.DomainDrivenDesigner.Application/Services/ContextAppService.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **ContextAppService** class

                ```csharp
                namespace Activator.DomainDrivenDesigner.Application.Services;

                public class BusinessModelPersistor
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **ContextAppService**, Implements: **IContextAppService**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Application.Services;`

            3. Inject below through constructor
            - **IContextRepository**
            - **IServiceProvider**

            4. Place Attributes
            - Decorate **ContextAppService** class with [ServiceLocate(typeof(ContextAppService))]
            - Decorate each method below with [LogTrace(typeof(*response))].

            5. Public async method signature: `Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)`

                Method **RetrieveContexts** logic: 
                ```mermaid
                    graph TB
                        subgraph RetrieveContexts
                            direction TB
                            start(("Start")) -->
                            |request: RetrieveContextAppRequest| loadAllContextInProject["`Load All **Context**s domain model by request.ProjectId`"] -->
                            return["`Return RetrieveContextAppResponse with loaded **Context**s`"]
                        end
                ```
            6. Public async method signature: `Task<CreateContextAppResponse> CreateContext(CreateContextAppRequest request)`

                Method **CreateContext** logic: 
                ```mermaid
                    graph TB
                        subgraph CreateContext
                            direction TB
                            start2(("Start")) --> 
                            |request: CreateContextAppRequest| newContext["New a **Context** domain model with Name and ProjectId from request"]-->
                            return2["`Return CreateContextAppResponse with **ContextId**`"]
                        end
                ```
            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Repositories:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Context//IConetxtRepository.cs")`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Context//Entities//Context.cs")`.
            - Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

            ## Reference AppRequests:
            - Execute `read_code_file("2-Application\Activator.DomainDrivenDesigner.Application\AppRequests\RetrieveContextAppRequest.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//CreateContextAppRequest.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//AppRequest.cs")`.

            ## Reference AppResponses:
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//RetrieveContextAppResponse.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//CreateContextAppResponse.cs")`.
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//AppResponse.cs")`.

            ## Output
            Only output full source code of **ContextAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.
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