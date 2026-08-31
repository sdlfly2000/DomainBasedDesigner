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

        //_actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory, "ornith:9b");
        _actionGeneratorAgent = new ActionGeneratorAgent(aIAgentClientFactory);
    }

    [Test]
    public async Task Convert_ShouldReturnConvertedClasses_WhenValidInstructionIsProvided()
    {
        // Arrange
        var instruction =
            """
            ## Generate complete C# code            
            File: **2-Application/Activator.DomainDrivenDesigner.Application.Services/ContextAppService.cs**

            ## Format:
            Write a C# **ContextAppService** class

            ```csharp
            public class ContextAppService
            {
                ### Your Code Fixed (Allman Style)
            }
            ```

            ## Rules:
            1. Class: **ContextAppService**, Implements: **IContextAppService**

            2. Inject below through constructor
            - **IDDDRepository**
            - **IServiceProvider**

            3. Place Attributes
            - Put Attribute [ServiceLocate(typeof(ContextAppService))] to **ContextAppService** class.
            - Put Attribute [LogTrace(typeof(*response))] to each method below.

            4. Public async method signature: ```Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)```

                Method **RetrieveContexts** logic: 
                ```mermaid
                    graph TB
                        subgraph main [RetrieveContexts]
                            direction TB
                            start(("Start"s)) -->
                            |request: RetrieveContextAppRequest| loadAllContext["Load All Context -> IDDDRepository.RetrieveContexts()"] -->
                            return["`Return **Context**`"]
                        end
                ```
            5. Public async method signature: ```Task<CreateContextAppResponse> CreateContext(CreateContextAppRequest request)```

                Method **CreateContext** logic: 
                ```mermaid
                    graph TB
                        subgraph main CreateContext[CreateContext]
                            direction TB
                            start2(("Start")) --> 
                            |request: CreateContextAppRequest| CreateContext["Create a Context -> IDDDRepository.CreateContexts(request.Name)"]-->
                            return2["`Return **ContextId**`"]
                        end
                ```
            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Interface Signatures:
            ```csharp
            Task<List<Domain.Entities.Context>> IDDDRepository.RetrieveContexts();
            Task<Guid>> IDDDRepository.CreateContext(string name);
            ```

            ## Reference Requests and Responses:
            ```mermaid
            classDiagram
                class AppRequest {
                    + Id: Guid
                }

                class AppResponse {
                    + RequestId： Guid
                    + Success: bool
                    + ErrorMessage: string?
                }

                class CreateContextAppRequest {
                    + Name: string
                }

                class CreateContextAppResponse {
                    + ContextId: Guid
                }

                class RetrieveContextAppRequest {

                }

                class RetrieveContextAppResponse {
                    + Contexts: List~Context~
                }

                %% Relationship
                AppRequest <|-- RetrieveContextAppRequest
                AppResponse <|-- RetrieveContextAppResponse

                AppRequest <|-- CreateContextAppRequest
                AppResponse <|-- CreateContextAppResponse

            ```

            ```csharp
            public record CreateContextAppRequest(Guid Id, string Name) : AppRequest(Id);
            public record RetrieveContextAppRequest(Guid Id) : AppRequest(Id);
            public record CreateContextAppResponse(Guid RequestId, Guid? ContextId, bool Success, string? ErrorMessage) : AppResponse(RequestId, Success, ErrorMessage);
            public record RetrieveContextAppResponse(Guid RequestId, List<Context>? Contexts, bool Success, string? ErrorMessage) : AppResponse(RequestId, Success, ErrorMessage);
            ```

            ## Output
            Only output full source code of **ContextAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.
            """;

        // Action
        var result = await _actionGeneratorAgent.Create(instruction, CancellationToken.None).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        Console.WriteLine(string.Concat("File: ", result.Result.file_path));
        Console.WriteLine(string.Concat("Content: ", Environment.NewLine, result.Result.content));
    }
}
