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
            File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer/Repositories/ContextRepository.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **ContextRepository** class
                ```csharp
                namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

                public class ContextRepository
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **ContextRepository**, Implements: **IContextRepository**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;`

            3. Inject below through constructor
            - **DomainDbContext**

            4. Place Attributes
            - Put Attribute [ServiceLocate(typeof(IContextRepository))] to **ContextRepository** class.

            5. Public async method signature: `Task<List<Domain.Entities.Context>> RetrieveContexts(Guid projectId)`

                Method **RetrieveContexts** logic: 
                ```mermaid
                    graph TB
                        subgraph main [RetrieveContexts]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - projectId: Guid| loadAllContextDatabaseEntity["`Load All **T_BUSINESS_CONTEXT**s where ProjectId == projectId`"] -->
                            mapToContext["`Map **T_BUSINESS_CONTEXT** database entity to **Context** doamin model`"] -->
                            return["`Return **Context**s`"]
                        end
                ```

            5. Public async method signature: `Task<Guid> CreateContext(string name, Guid projectId)`

                Method **CreateContext** logic: 
                ```mermaid
                    graph TB
                        subgraph main [CreateContext]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - name: string, 
                            - projectId: Guid| newContext["`New a **T_BUSINESS_CONTEXT** with Name and ProjectId, Guid.NewGuid()`"] -->
                            AddToContext["`Add it to **T_BUSINESS_CONTEXT**`"] -->
                            return["`Return ContextId`"]
                        end
                ```

            5. Public async method signature: `Task<Guid> UpdateContext(Context context)`

                Method **UpdateContext** logic: 
                ```mermaid
                    graph TB
                        subgraph main [UpdateContext]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - context: Context| loadContextDatabseEntity["Load existing **T_BUSINESS_CONTEXT** from database by context.Id"] -->
                            DomainEntityNotFoundException -->
                            updateContext["Update **T_BUSINESS_CONTEXT** with values from context"] -->
                            return["`Return ContextId`"]
                        end
                ```

            ## Private Method:
            ```csharp
            private Context mapToContext(T_BUSINESS_CONTEXT rowContext);
            ```

            ## Persistence Rules:
            - **Self-Contained Commit:** Call `await _dbContext.SaveChangesAsync().ConfigureAwait(false)` immediately after adding the entity to ensure change state tracking is flushed to SQL Server before returning.

            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Exceptions:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Exceptions//DomainEntityNotFoundException.cs")`.

            ## Reference Database Context:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Context//DomainDbContext.cs")`.

            ## Reference Database Entities:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_CONTEXT.cs")`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Context//Entities//Context.cs")`.
            - Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

            ## Output
            Only output full source code of **ContextRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.
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