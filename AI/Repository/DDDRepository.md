## Generate complete C# code 
File: **4-Infrastructure/Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories/DDDRepository.cs**

## Format:
Write a C# **DDDRepository** class

```csharp
public class DDDRepository
{
    ### Your Code Fixed (Allman Style)
}
```

## Rules:
1. Class: **DDDRepository**, Implements: **IDDDRepository**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Repositories;`

3. Inject below through constructor
- **DomainDbContext**

4. Place Attributes
- Put Attribute [ServiceLocate(typeof(IDDDRepository))] to **DDDRepository** class.

5. Public async method signature: `Task<List<Domain.Entities.Context>> RetrieveContexts(Guid projectId)`

    Method **RetrieveContexts** logic: 
    ```mermaid
        graph TB
            subgraph main [RetrieveContexts]
                direction TB
                start(("Start"s)) -->
                |Argument: 
                - projectId: Guid| loadAllContext["Load All Contexts where ProdjectId == projectId"] -->
                mapContext["Map T_BUSINESS_CONTEXT to Context -> Map(T_BUSINESS_CONTEXT)"] -->
                return["`Return **Context**s`"]
            end
    ```

6. Public async method signature: `Task<Guid> CreateContext(string name, Guid projectId)`

    Method **CreateContext** logic: 
    ```mermaid
        graph TB
            subgraph main [CreateContext]
                direction TB
                start(("Start"s)) -->
                |Argument: 
                - name: string, 
                - projectId: Guid| newContext["New a T_BUSINESS_CONTEXT with Name and ProjectId, Guid.NewGuid() -> Id and DateTime.UtcNow -> CREATED_UTC"] -->
                AddToContext["`Add it to **T_BUSINESS_CONTEXT**`"] -->
                return["`Return ContextId`"]
            end
    ```
## Private Method:
- private method signature: `private Domain.Entities.Context Map(T_BUSINESS_CONTEXT rowBusinessContext)`

## Ignore Exception Handler since it is included in LogTrace Attribute

## Reference Interface Signatures:

**DomainDbContext**:
```csharp
public virtual DbSet<T_BUSINESS_CONTEXT> T_BUSINESS_CONTEXTs { get; set; }
```
## Reference Database Entities:
```csharp
public partial class T_BUSINESS_CONTEXT
{
    public Guid ID { get; set; }

    public string? NAME { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public Guid? T_PROJECT_ID { get; set; }
}
```

## Reference Domain Entities:
```csharp
public class Context(Guid ID) : EntityBase(ID)
{
    public string? Name { get; set; }
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
Only output full source code of **DDDRepository.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.