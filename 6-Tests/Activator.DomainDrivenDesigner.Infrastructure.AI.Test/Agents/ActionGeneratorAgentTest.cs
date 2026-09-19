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
            File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories/BusinessModelRepository.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **BusinessModelRepository** class

                ```csharp
                namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

                public class BusinessModelRepository
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **BusinessModelRepository**, Implements: **IBusinessModelRepository**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;`

            3. Inject below through constructor
            - **DomainDbContext**

            4. Place Attributes
            - Put Attribute [ServiceLocate(typeof(IBusinessModelRepository))] to **BusinessModelRepository** class.

            5. Public async method signature: `Task<BusinessModel> RetrieveBusinessModelsById(Guid businessModelId)`

                Method **RetrieveBusinessModelsById** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Create Project]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - businessModelId: Guid| LoadBusinessModelFromDb["`Eagerly load **T_BUSINESS_MODEL** including **CONTEXT** from DomainDbContext. Note only single T_BUSINESS_MODEL via businessModelId, or throw **DomainEntityNotFoundException**`"] -->
                            MapBusinessModelFromDb["`Map loaded **T_BUSINESS_MODEL** database entity into **BusinessModel** domain model`"] -->
                            return["`Return mapped **BusinessModel**`"]
                        end
                ```

            5. Public async method signature: `Task<Guid> UpdateBusinessModels(BusinessModel model)`

                Method **UpdateBusinessModels** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Create Project]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - model: BusinessModel| LoadBusinessModelFromDb["`Load **T_BUSINESS_MODEL** DomainDbContext by model.Id`"] -->
                            Exist{"`Exist?`"} --> 
                            PersistDoaminModelToDbEntity["`Persist **BusinessModel** doamin model to loaded **T_BUSINESS_MODEL** database entity`"] -->
                            SaveChange["`Save changes to database`"] -->
                            return["`Return updated model Id`"]

                            %% Exceptions
                            Exist -->|no| throwException["`Throw **DomainEntityNotFoundException**`"]
                        end
                ```

            ## Private Method:
            ```csharp
            private BusinessModel Map(T_BUSINESS_MODEL rowBusinessModel);
            private T_BUSINESS_MODEL Persist(BusinessModel model);
            ```

            ## Persistence Rules:
            - **Self-Contained Commit:** Call `await _dbContext.SaveChangesAsync().ConfigureAwait(false)` immediately after adding the entity to ensure change state tracking is flushed to SQL Server before returning.

            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Exceptions:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Exceptions//DomainEntityNotFoundException.cs")`.

            ## Reference Database Context:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Context//DomainDbContext.cs")`.

            ## Reference Database Entities:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_MODEL.cs")`.
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_CONTEXT.cs")`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Entities//BusinessModel.cs")`.
            - Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

            ## Output
            Only output full source code of **BusinessModelRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.
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
