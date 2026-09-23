## Generate complete C# code 
File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer/Repositories/ProjectRepository.cs**

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

5. Public async method signature: `Task<List<Project>> RetrieveFullProjects()`

    Method **RetrieveFullProjects** logic: 
    ```mermaid
        graph TB
            subgraph main [Retrieve Full Projects]
                direction TB
                start(("Start")) --> load["`Eagerly load **T_PROJECT**s including **T_REQUIREMENT**s from DomainDbContext`"] -->
                MapToProjectDomainModel["`Map each **T_PROJECT** into a **Project** domain model, mapping nested **T_REQUIREMENT**s collections. Do not load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT**`"] -->
                return["`Return the list of mapped **Project** domain objects`"]
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

                DomainEntityNotFoundException -->

                %% Do NOT load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT** 
                MapToProjectDomainModel["`Map loaded **T_PROJECT** into a **Project** domain model.`"] -->

                return["`Return the mapped **Project** domain objects`"]
            end
    ```

5. Public async method signature: `Task<List<Requirement>> RetrieveRequirementByProjectId(Guid projectId)`

    Method **RetrieveRequirementByProjectId** logic: 
    ```mermaid
        graph TB
            subgraph main [Retrieve Requirements by Project Id]
                direction TB
                start(("Start")) -->

                %% Include referencing **T_REQUIREMENT**s  
                |Args: - projectId: Guid | load["`Load **T_PROJECT**s by projectId, from DomainDbContext`"] -->

                DomainEntityNotFoundException -->

                %% Do NOT load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT** 
                MapToRequirementDomainModel["`Map **T_REQUIREMENT**s nested in loaded **T_PROJECT** into a **Requirement** domain models.`"] -->

                return["`Return the mapped **Requirement** domain models`"]
            end
    ```

5. Public async method signature: `Task<Requirement> RetrieveRequirementById(Guid requirementId)`

    Method **RetrieveRequirementById** logic: 
    ```mermaid
        graph TB
            subgraph main [Retrieve Requirements by Id]
                direction TB
                start(("Start")) -->

                |Args: - requirementId: Guid | load["`Load **T_REQUIREMENT** by requirementId from DomainDbContext`"] -->

                DomainEntityNotFoundException -->

                %% Do NOT load nested **T_BUSINESS_ACTION** nor **T_BUSINESS_MODEL** in **T_REQUIREMENT** 
                MapToRequirementDomainModel["`Map **T_REQUIREMENT** database entity into a **Requirement** domain models.`"] -->

                return["`Return the mapped **Requirement** domain models`"]
            end
    ```

## Create Private Methods:
```csharp
private Project MapToProjectDomainModel(T_PROJECT rowProject);
private Requirement MapToRequirementDomainModel(T_REQUIREMENT rowRequirment);
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

## Output
Only output full source code of **ProjectRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.