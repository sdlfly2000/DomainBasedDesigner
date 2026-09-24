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
            File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer/Persistors/BusinessModelPersistor.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **BusinessModelPersistor** class

                ```csharp
                namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors;

                public class BusinessModelPersistor
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **BusinessModelPersistor**, Implements: **IBusinessModelPersistor**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors;`

            3. Inject below through constructor
            - **DomainDbContext**

            4. Place Attributes
            - Decorate **BusinessModelPersistor** class with [ServiceLocate(typeof(IBusinessModelPersistor))].

            5. Public async method signature: `Task<Guid> CreateBusinessModel(BusinessModel model, Guid requirementId)`

                Method **CreateBusinessModel** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Create BusinessModel]
                            direction TB
                            start(("Start")) --> 
                            |Args: 
                            - businessModel: BusinessModel 
                            - requirementId: Guid | PersistToBusinessModelDatabaseEntity["`New a **T_BUSINESS_MODEL** database entity, and Persist **BusinessModel** domain model passed in to new created **T_BUSINESS_MODEL** database entity`"] -->
                            AddToBusinessModel["`Add new created **T_BUSINESS_MODEL** to **T_BUSINESS_MODEL**s in DomainDbContext`"] -->
                            return["`Return the id of **BusinessModel** domain objects`"]
                        end
                ```

            5. Public async method signature: `Task<Guid> UpdateBusinessModel(BusinessModel model)`

                Method **UpdateBusinessModel** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Update BusinessModel]
                            direction TB
                            start(("Start")) --> 
                            |Args: 
                            - businessModel: BusinessModel | loadBusinessModelDatabaseEntityById["`Load **T_BUSINESS_MODEL** database entity by **BusinessModel**.ID`"] -->
                            DomainEntityNotFoundException -->

                            PersistToBusinessModelDatabaseEntity["`Persist **BusinessModel** domain model passed in to loaded **T_BUSINESS_MODEL** database entity`"] -->
                            UpdateToBusinessModelDatabaseEntity["`Update **T_BUSINESS_MODEL** in DomainDbContext`"] -->
                            return["`Return the Id of **BusinessModel** domain objects`"]
                        end
                ```

            ## Persistence Rules:
            - **Self-Contained Commit:** Call `await _dbContext.SaveChangesAsync().ConfigureAwait(false)` immediately after adding the entity to ensure change state tracking is flushed to SQL Server before returning.

            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Database Context:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Context//DomainDbContext.cs")`.

            ## Reference Database Entities:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_MODEL.cs")`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//BusinessModel//Entities//BusinessModel.cs")`.
            - Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

            ## Reference Exception:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Exceptions//DomainEntityNotFoundException.cs")`.

            ## Output
            Only output full source code of **BusinessModelPersistor.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.
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