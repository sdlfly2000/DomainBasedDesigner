using Activator.DomainDrivenDesigner.Infrastructure.AI.Agents;
using Activator.DomainDrivenDesigner.Infrastructure.AI.Client;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Activator.DomainDrivenDesigner.Infrastructure.AI.Test.Agents;

public class ActionGeneratorAgentTest
{
    private ActionGeneratorAgent _actionGeneratorAgent;

    [SetUp]
    public void Setup()
    {
        var aiOptions = Options.Create(new AIOptions
        {
            Endpoint = "http://homeserver4:11434"
        });

        var aIAgentClientFactory = new AIAgentClientFactory(aiOptions);

        //_actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory, "deepseek-coder:6.7b");
        _actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory);
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

                             |request: CreateProjectAppRequest| newProject["`Create a new **Project** -> Project.Create(request.ProjectName, request.ProjectDescription`"] -->

                             %% {/* ProjectRepository.CreateProject(project).ConfigureAwait(false) */} 
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

                             %% {/* IProjectRepository.RetrieveFullProjects().ConfigureAwait(false) */} 
                             |request: RetrieveFullProjectAppRequest| retrieveAllProjects["`Retrieve all **Project**s`"] -->

                             %% {/* return new RetrieveFullProjectAppResponse(request.Id, projects, true, null)  */}
                             return["`Return **RetrieveFullProjectAppResponse**`"]
                         end
                 ```

             5. Public async method signature: ` Task<RetrieveBusinessModelsAppResponse> RetrieveProjectBusinessModels(RetrieveBusinessModelsAppRequest request)`

                 Method **RetrieveProjectBusinessModels** logic: 
                 ```mermaid
                     graph TB
                         subgraph main [Retrieve Project Business Models]
                             direction TB
                             start(("Start"s)) -->

                             %% {/* IProjectRepository.RetrieveBusinessModelsByProjectId(request.ProjectId).ConfigureAwait(false) */} 
                             |request: RetrieveBusinessModelsAppRequest| retrieveBusinessModelViaProjectId["`Retrieve **BusinessModel** by ProjectId`"] -->

                             %% {/* return new RetrieveBusinessModelsAppResponse(request.Id, businessModels, true, null)  */} 
                             return["`Return **RetrieveBusinessModelsAppResponse**`"]
                         end
                 ```
             ## Context Boundaries:
             - **Ignore Exception Handling:** Omit manual try-catch wrappers since exceptions are decoupled via the infrastructure `LogTrace` attribute tier.
             - **Asynchronous Execution:** Every data tier interaction must map via explicit asynchronous operations utilizing `ConfigureAwait(false)`.

             ## Reference Dependency Interface Signatures:
             ```csharp
             public interface IProjectRepository
             {
                 Task<Guid?> CreateProject(Project project);
                 Task<List<Project>> RetrieveFullProjects();
                 Task<List<BusinessModel>> RetrieveBusinessModelsByProjectId(Guid ProjectId);
             }
             ```

             ## Reference Requests and Responses:
             ```csharp
             public abstract record AppRequest(Guid Id);
             public record RetrieveBusinessModelsAppRequest(Guid Id, Guid ProjectId) : AppRequest(Id);
             public record CreateProjectAppRequest(Guid Id, string ProjectName, string ProjectDescription) : AppRequest(Id);
             public record RetrieveFullProjectAppRequest(Guid Id) : AppRequest(Id);

             public abstract record AppResponse(Guid RequestId, bool Success, string? ErrorMessage);
             public record RetrieveBusinessModelsAppResponse(Guid RequestId, List<BusinessModel>? BusinessModels, bool Success, string? ErrorMessage) 
                 : AppResponse(RequestId, Success, ErrorMessage);
             public record CreateProjectAppResponse(Guid RequestId, bool Success, string? ErrorMessage) 
                 : AppResponse(RequestId, Success, ErrorMessage);
             public record RetrieveFullProjectAppResponse(Guid RequestId, List<Project>? Projects, bool Success, string? ErrorMessage) 
                 : AppResponse(RequestId, Success, ErrorMessage);
             ```

             ## Output
             Only output full source code of **ProjectAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.
            """;

        // Action
        var result = await _actionGeneratorAgent.Create(instruction, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        Console.WriteLine(string.Concat("File: ", result.Result.file_path));
        Console.WriteLine(string.Concat("Content: ", Environment.NewLine, result.Result.content));
    }
}
