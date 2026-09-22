## Generate complete C# code 
File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer/Persistors/ProjectPersistor.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **ProjectPersitor** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors;

    public class ProjectPersistor
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **ProjectPersistor**, Implements: **IProjectPersistor**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Persistors;`

3. Inject below through constructor
- **DomainDbContext**

4. Place Attributes
- Decorate **ProjectPersistor** class with [ServiceLocate(typeof(IProjectPersistor))].

5. Public async method signature: `Task<Guid?> CreateProject(Project project)`

    Method **CreateProject** logic: 
    ```mermaid
        graph TB
            subgraph main [Create Project]
                direction TB
                start(("Start")) -->
                |Argument: 
                - project: Project| NewProjectDatabaseEntity["`New a **T_PROJECT** with **Project** passed in, and assign CreatedOnUtc of **Project** doamin model to **T_PROJECT** database entity, do not handle **Requirement**s in project.`"] -->
                addProjectToDatabase["`Add new **T_PROJECT** to DomainDbContext.T_PROJECTs`"] -->
                return["`Return **Project**'s Id`"]
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
                - projectId: Guid | NewRequirementDatabaseEntity["`New a **T_REQUIREMENT** database entity with **Requirement**, and assign projectId to the new created **T_REQUIREMENT**, and assign **Requirement**.ID and CreatedOnUtc  to the new created **T_REQUIREMENT**`"] -->

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
                - requirement: Requirement | load["`Load **T_REQUIREMENT** database entity by ID of requirement pass in.`"] -->
                 
                DomainEntityNotFoundException -->

                %% Do not persist BusinessModels nor BusinessActions in *Requirement*
                MapToRequirementDatabaseEntity["`Map **Requirement** pass in to loaded **T_REQUIREMENT** database entity in DomainDbContext`"] -->

                UpdateAndSave["`Update **T_REQUIREMENT** database entity to Db`"] --> 
                return["`Return the id of **Requirement** domain model`"]
            end
    ```

## Create Private Methods:
```csharp
private T_PROJECT NewProjectDatabaseEntity(Project project);
private void MapToRequirementDatabaseEntity(T_REQUIREMENT rowrequirement, Requirement requirement);
private T_REQUIREMENT NewRequirementDatabaseEntity(Requirement requirement, Guid projectId);
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
Only output full source code of **ProjectPersistor.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.