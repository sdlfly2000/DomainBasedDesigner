## Generate complete C# code 
File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories/ProjectRepository.cs**

## Format:
Write a C# **ProjectRepository** class

```csharp
namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;
public class ProjectRepository
{
    ### Your Code Fixed (Allman Style)
}
```

## Rules:
1. Class: **ProjectRepository**, Implements: **IProjectRepository**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;`

3. Inject below through constructor
- **DomainDbContext**

4. Place Attributes
- Put Attribute [ServiceLocate(typeof(IProjectRepository))] to **ProjectRepository** class.

5. Public async method signature: `Task<Guid?> CreateProject(Project project)`

    Method **CreateProject** logic: 
    ```mermaid
        graph TB
            subgraph main [Create Project]
                direction TB
                start(("Start"s)) -->
                |Argument: 
                - project: Project| newProject["New a **T_PROJECT** and mapped by project passed in"] -->
                addProjectToDb["Add new **T_PROJECT** to DomainDbContext.T_PROJECTs"] -->
                return["`Return **Project**'s Id`"]
            end
    ```

5. Public async method signature: `Task<List<Project>> RetrieveFullProjects()`

    Method **RetrieveFullProjects** logic: 
    ```mermaid
        graph TB
            subgraph main [CRetrieve Full Projects]
                direction TB
                start(("Start"s)) -->

                |Argument: 
                - null| loadProjects["`Load **T_PROJECT**s, including **T_REQUIREMENTs**`"] -->

                ForEachProjectLoaded["`Foreach loaded **T_PROJECT**`"] -->

                %% {/* Map(T_PROJECT)*/}
                MapToProject["`Map loaded **T_PROJECT** to **Project**`"] -->

                %% {/* Map(T_REQUIREMENT)*/}
                MapToRequirement["`Map **T_REQUIREMENT**s from loaded **T_PROJECT**. to **Requirement**s`"] -->

                AddRequirementToProject["`Add **Requirement**s to **Project**`"] -->

                return["`Return **Project**s`"]
            end
        %% relationship
        AddRequirementToProject --> ForEachProjectLoaded
    ```

## Private Method:
```csharp
private Project Map(T_PROJECT rowProject)
{
    var project = new Project(rowProject.ID, rowProject.NAME)
    {
        Description = rowProject.DESCRIPTION,
        CreatedOnUtc = rowProject.CREATED_UTC
    };
    return project;
}

private Requirement Map(T_REQUIREMENT rowRequirment)
{
    var requirement = new Requirement(rowRequirment.ID)
    {
        Description = rowRequirment.DESCRIPTION,
        CreatedOnUtc = rowRequirment.CREATE_UTC
    };

    return requirement;
}
```

## Ignore Exception Handler since it is included in LogTrace Attribute

## Reference Interface Signatures:

## Reference Database Entities:
```csharp
public partial class T_PROJECT
{
    public Guid ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public virtual ICollection<T_BUSINESS_CONTEXT> T_BUSINESS_CONTEXTs { get; set; } = new List<T_BUSINESS_CONTEXT>();

    public virtual ICollection<T_REQUIREMENT> T_REQUIREMENTs { get; set; } = new List<T_REQUIREMENT>();
}

public partial class T_REQUIREMENT
{
    public Guid ID { get; set; }

    public string? DESCRIPTION { get; set; }

    public DateTime CREATE_UTC { get; set; }

    public Guid? PROJECT_ID { get; set; }

    public virtual T_PROJECT? PROJECT { get; set; }
}
```

## Reference Domain Entities:
```csharp
public class Project(Guid ID, string ProjectName) : EntityBase(ID)
{
    public List<Requirement> Requirements { get; set; } = [];

    public string Name { get; } = ProjectName;

    public string? Description { get; set; }

    public static Project Create(string ProjectName, string ProjectDescription)
    {
        return new Project(Guid.NewGuid(), ProjectName)
        {
            Description = ProjectDescription,
            CreatedOnUtc = DateTime.UtcNow,
        };
    }
}
```

## Output
Only output full source code of **ContextRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.