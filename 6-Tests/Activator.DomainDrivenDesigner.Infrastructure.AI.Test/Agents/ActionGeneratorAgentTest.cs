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
            File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories/ProjectRepository.cs**

            ## Format:
            - **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

            - Write a C# **ProjectRepository** class

                ```csharp
                namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;

                public class ProjectRepository
                {
                    // Your Full Code Implementation (Allman Style including Using statements)
                }
                ```

            ## Rules:
            1. Class: **ProjectRepository**, Implements: **IProjectRepository**

            2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;`

            3. Inject below through constructor
            - **DomainDbContext**

            4. Place Attributes
            - Decorate **ProjectRepository** class with [ServiceLocate(typeof(IProjectRepository))].

            5. Public async method signature: `Task<Guid?> CreateProject(Project project)`

                Method **CreateProject** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Create Project]
                            direction TB
                            start(("Start")) -->
                            |Argument: 
                            - project: Project| newProject["`New a **T_PROJECT** with **Project** passed in, do not handle **Requirement**s in project.`"] -->
                            addProjectToDb["`Add new **T_PROJECT** to DomainDbContext.T_PROJECTs`"] -->
                            return["`Return **Project**'s Id`"]
                        end
                ```

            5. Public async method signature: `Task<List<Project>> RetrieveFullProjects()`

                Method **RetrieveFullProjects** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Retrieve Full Projects]
                            direction TB
                            start(("Start")) --> load["`Eagerly load **T_PROJECT**s including **T_REQUIREMENT**s from DomainDbContext`"]
                            load --> map["`Map each **T_PROJECT** into a **Project** domain model, mapping nested **T_REQUIREMENT**s collections. Do not load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT**`"]
                            map --> return["`Return the list of mapped **Project** domain objects`"]
                        end
                ```

            5. Public async method signature: `Task<Project> RetrieveProjectById(Guid projectId)`

                Method **RetrieveProjectById** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Retrieve Project by Id]
                            direction TB
                            start(("Start")) -->

                            %% Include referencing **T_REQUIREMENT**s  
                            |Args: - projectId: Guid | load["`Load **T_PROJECT**s by projectId, from DomainDbContext`"] -->

                            ThrowExceptionIfNotExist["Throw DomainEntityNotFoundException"] -->

                            %% Do NOT load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT** 
                            map["`Map loaded **T_PROJECT** into a **Project** domain model.`"] -->

                            return["`Return the mapped **Project** domain objects`"]
                        end
                ```

            5. Public async method signature: `Task<Guid?> CreateRequirement(Requirement requirement, Guid projectId)`

                Method **CreateRequirement** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Create Requirement]
                            direction TB
                            start(("Start")) --> 
                            |Args: 
                            - requirement: Requirement 
                            - projectId: Guid | newRequirementDbEntity["`New a **T_REQUIREMENT** database entity with **Requirement**, and assign projectId to the new created **T_REQUIREMENT**, and assign **Requirement**.ID to the new created **T_REQUIREMENT**`"] -->

                            AddToRequirements["`Add new created **T_REQUIREMENT** to **T_REQUIREMENT**s in DomainDbContext`"] -->

                            return["`Return the id of **Requirement** domain objects`"]
                        end
                ```

            5. Public async method signature: `Task<Guid?> UpdateRequirement(Requirement requirement);`

                Method **UpdateRequirement** logic: 
                ```mermaid
                    graph TB
                        subgraph main [Update Requirement]
                            direction TB
                            start(("Start")) --> 
                            |Args: 
                            - requirement: Requirement | loadRequirementDbEntity["`Load the **T_REQUIREMENT** database entity with **Requirement**.ID`"] -->

                            ThrowExceptionIfNotExist["Throw DomainEntityNotFoundException"] -->

                            %% Do not persist BusinessModels nor BusinessActions in *Requirement*
                            UpdateRequirementDbEntity["`Persist **Requirement** to  **T_REQUIREMENT** in DomainDbContext`"] -->

                            return["`Return the id of **Requirement** domain model`"]
                        end
                ```

            ## Create Private Methods:
            ```csharp
            private T_PROJECT Persist(Project project);
            private T_REQUIREMENT Persist(Requirement requirement);
            private T_REQUIREMENT Persist(Requirement requirement, Guid projectId);
            private Project Map(T_PROJECT rowProject);
            private Requirement Map(T_REQUIREMENT rowRequirment);
            ```

            ## Persistence Rules:
            - **Self-Contained Commit:** Call `await _dbContext.SaveChangesAsync().ConfigureAwait(false)` immediately after adding the entity to ensure change state tracking is flushed to SQL Server before returning.

            ## Ignore Exception Handler since it is included in LogTrace Attribute

            ## Reference Database Context:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Context//DomainDbContext.cs")`.

            ## Reference Database Entities:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_PROJECT.cs")`.
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_REQUIREMENT.cs")`.

            ## Reference Domain Models:
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Project//Entities//Project.cs")`.
            - Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Project//Entities//Requirement.cs")`.
            - Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

            ## Reference Exception:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Exceptions//DomainEntityNotFoundException.cs")`.

            ## Reference ProjectRepository If Exists:
            - Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Repositories//ProjectRepository.cs")`.

            ## Output
            Only output full source code of **ProjectRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.
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
