## Generate complete C# code 
File: **2-Application/Activator.DomainDrivenDesigner.Application.Services/ContextAppService.cs**

## Format:
Write a C# **ContextAppService** class

```csharp
public class ContextAppService
{
    ### Your Code Fixed (Allman Style)
}
```

## Rules:
1. Class: **ContextAppService**, Implements: **IContextAppService**

2. Inject below through constructor
- **IDDDRepository**
- **IServiceProvider**

3. Public async method signature: ```Task<RetrieveContextAppResponse> RetrieveContexts(RetrieveContextAppRequest request)```

    Method **RetrieveContexts** logic: 
    ```mermaid
        graph TB
            subgraph main [Activator.DomainDrivenDesigner.Application.Services.ContextAppService]
                direction TB
                start(("Start"s)) -->
                |request: RetrieveContextAppRequest| loadAllContext["Load All Context -> IDDDRepository.RetrieveContexts()"] -->
                return["`Return **Context**`"]
            end
    ```

## Reference Interface Signatures:
```csharp
Task<List<Domain.Entities.Context>> IDDDRepository.RetrieveContexts();
```

## Output
Only output full source code of **ContextAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.