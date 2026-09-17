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

        //_actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory, "qwen2.5-coder:3b-instruct", true);
        _actionGeneratorAgent = new ActionGeneratorAgent(_logger, aIAgentClientFactory, applyQwenToolFix: false);
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
                           start(("Start"s)) -->
                           |Argument: 
                           - businessModelId: Guid| LoadBusinessModelFromDb["`Eagerly load **T_BUSINESS_MODEL** including **CONTEXT** from DomainDbContext. Note only single T_BUSINESS_MODEL via businessModelId, or throw **DomainEntityNotFoundException**`"] -->
                           MapBusinessModelFromDb["`Map loaded **T_BUSINESS_MODEL** database entity into **BusinessModel** domain model`"] -->
                           return["`Return mapped **BusinessModel**`"]
                       end
               ```

           ## Private Method:
           ```csharp
           private BusinessModel Map(T_BUSINESS_MODEL rowBusinessModel)
           ```

           ## Persistence Rules:
           - **Self-Contained Commit:** Call `await _dbContext.SaveChangesAsync().ConfigureAwait(false)` immediately after adding the entity to ensure change state tracking is flushed to SQL Server before returning.

           ## Ignore Exception Handler since it is included in LogTrace Attribute

           ## Reference Exceptions:
           ```csharp
                      public class DomainEntityNotFoundException(string message) : Exception(message)
           {
               public static void ThrowIfNull<TEntity>(Guid entityId, [NotNull] TEntity? entity)
               { 
                   if (entity == null)
                   {
                       throw new DomainEntityNotFoundException($"Entity of {typeof(TEntity).Name} with ID({entityId}) not found.");
                   }
               }
           }
           ```

           ## Reference Database Entities:
           ```csharp
           public partial class T_BUSINESS_MODEL
           {
               public Guid ID { get; set; }

               public string? NAME { get; set; }

               public Guid? REQUIREMENT_ID { get; set; }

               public string? RAW_DESCRIPTION { get; set; }

               public Guid? CONTEXT_ID { get; set; }

               public DateTime CREATED_UTC { get; set; }

               public virtual T_BUSINESS_CONTEXT? CONTEXT { get; set; }
           }

           public partial class T_BUSINESS_CONTEXT
           {
               public Guid ID { get; set; }

               public string? NAME { get; set; }

               public DateTime CREATED_UTC { get; set; }

               public Guid? T_PROJECT_ID { get; set; }
           }
           ```

           ## Reference Domain Models:
           ```csharp
           public class BusinessModel(Guid ID) : EntityBase(ID)
           {
               public string? Name { get; set; }

               public string? ContentMermaid { get; set; }

               public Guid? ContextId { get; set; }    
           }

           public abstract class EntityBase
           {
               protected EntityBase(Guid ID)
               {   
                   Id = ID;
               }

               public Guid Id { get; set; }

               public DateTime CreatedOnUtc { get; set; }
           }
           ```
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
