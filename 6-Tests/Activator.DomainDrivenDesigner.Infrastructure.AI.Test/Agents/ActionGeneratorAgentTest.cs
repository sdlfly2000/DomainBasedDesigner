using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class ActionGeneratorAgentTest
{
    private ActionGeneratorAgent _actionGeneratorAgent;
    private ILogger _logger;

    [SetUp]
    public void Setup()
    {
        var serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        _logger = new SerilogLoggerFactory(serilogLogger).CreateLogger<ActionGeneratorAgentTest>();

        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions, _logger);

        //_actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory, "ornith:9b", true);
        _actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory);
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidInstructionIsProvided()
    {
        // Arrange
        var instruction =
            """
             ## Generate complete C# code 
             File: **2-Application/Activator.DomainDrivenDesigner.Application.Services/ProjectAppService.cs**

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
             - **IServiceProvider** (store in a private readonly field `_serviceProvider`)

             4. Place Attributes
             - Decorate **ProjectAppService** class with [ServiceLocate(typeof(ProjectAppService))].
             - Decorate each public method with `[LogTrace(typeof({ResponseTypeName}))]`, replacing `{ResponseTypeName}` with the corresponding concrete return type. Ensure the `[LogTrace]` attribute target type references the underlying response record, *not* the wrapping `Task<>` type.

             5. Public async method signature: `Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)`

                 Method **Create** logic: 
                 ```mermaid
                     graph TB
                         subgraph main [Create Project]
                             direction TB
                             start(("Start"s)) -->                

                             |request: CreateProjectAppRequest| newProject["`Create a new **Project**`"] -->

                             CreateProject["`Create the **Project** in Db`"] -->

                             %% {/* return new CreateProjectAppResponse(request.Id, true, null)  */}
                             return["`Return **CreateProjectAppResponse**`"]
                         end
                 ```

             5. Public async method signature: `Task<RetrieveFullProjectAppResponse> RetrieveFullProjects(RetrieveFullProjectAppRequest request)`

                 Method **RetrieveFullProjects** logic: 
                 ```mermaid
                     graph TB
                         subgraph main [Retrieve Full Projects]
                             direction TB
                             start(("Start"s)) -->

                             |request: RetrieveFullProjectAppRequest| retrieveAllProjects["`Retrieve all **Project**s`"] -->

                             %% {/* return new RetrieveFullProjectAppResponse(request.Id, projects, true, null)  */}
                             return["`Return **RetrieveFullProjectAppResponse**`"]
                         end
                 ```

             ## Context Boundaries:
             - **Ignore Exception Handling:** Omit manual try-catch wrappers since exceptions are decoupled via the infrastructure `LogTrace` attribute tier.
             - **Asynchronous Execution:** Every data tier interaction must map via explicit asynchronous operations utilizing `ConfigureAwait(false)`.

             ## Reference Domain Entities:
             - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Entities//Project.cs")`.
             
             ## Reference Dependency Interface Signatures:
             - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Repositories//IProjectRepository.cs")`.

             ## Reference Requests and Responses:
             - Execute `read_code_file("2-Application\Activator.DomainDrivenDesigner.Application\AppRequests\CreateProjectAppRequest.cs")`.
             - Execute `read_code_file("2-Application\Activator.DomainDrivenDesigner.Application\AppResponses\CreateProjectAppResponse.cs")`.

             ## Reference ProjectAppService.cs if existing:
            - Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//Services//ProjectAppService.cs")`.
     
             ## Output
             Only output full source code of **ProjectAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.
            """;

        // Action
        var result = await _actionGeneratorAgent.Create(instruction, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        _logger.LogInformation(result);
        //Console.WriteLine(string.Concat("File: ", result.Result.file_path));
        //Console.WriteLine(string.Concat("Content: ", Environment.NewLine, result.Result.content));
    }
}
