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
- Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Exceptions//DomainEntityNotFoundException.cs")`.

## Reference Database Entities:
- Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_MODEL.cs")`.
- Execute `read_code_file("4-Infrastructure//Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer//Entities//T_BUSINESS_CONTEXT.cs")`.

## Reference Domain Models:
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Entities//BusinessModel.cs")`.
- Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

## Output
Only output full source code of **BusinessModelRepository.cs** in C# format, no other text. Use async/await correctly and Use *ConfigureAwait(false)* for each async call.
