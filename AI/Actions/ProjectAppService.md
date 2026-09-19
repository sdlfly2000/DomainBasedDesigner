## Generate complete C# code 
Target File: **2-Application/Activator.DomainDrivenDesigner.Application.Services/ProjectAppService.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **ProjectAppService** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Application.Services;

    public class ProjectAppService
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **ProjectAppService**, Implements: **IProjectAppService**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Application.Services;`

3. Inject below through constructor
- **IProjectRepository** (store in a private readonly field `_projectRepository`)
- **IServiceProvider** (store in a private readonly field `_serviceProvider`)

4. Place Attributes
- Decorate **ProjectAppService** class with [ServiceLocate(typeof(ProjectAppService))].
- Decorate each public method with `[LogTrace(typeof({ResponseTypeName}))]`, replacing `{ResponseTypeName}` with the corresponding concrete return type. Ensure the `[LogTrace]` attribute target type references the underlying response record, *not* the wrapping `Task<>` type.

5. Public async method signature: `Task<CreateProjectAppResponse> Create(CreateProjectAppRequest request)`

    Method **Create** logic: 
    ```mermaid
        graph TB
            subgraph main [Create Project]
                direction TB
                start(("Start")) -->
                |request: CreateProjectAppRequest| newProject["`Create a new **Project**`"] -->
                return["`Return **CreateProjectAppResponse**`"]
            end
    ```

5. Public async method signature: `Task<RetrieveFullProjectAppResponse> RetrieveFullProjects(RetrieveFullProjectAppRequest request)`

    Method **RetrieveFullProjects** logic: 
    ```mermaid
        graph TB
            subgraph main [Retrieve Full Projects]
                direction TB
                start(("Start")) -->
                |request: RetrieveFullProjectAppRequest| retrieveAllProjects["`Retrieve all **Project**s`"] -->
                return["`Return **RetrieveFullProjectAppResponse**`"]
            end
    ```

## Context Boundaries:
- **Ignore Exception Handling:** Omit manual try-catch wrappers since exceptions are decoupled via the infrastructure `LogTrace` attribute tier.
- **Asynchronous Execution:** Every data tier interaction must map via explicit asynchronous operations utilizing `ConfigureAwait(false)`.

## Reference Domain Models:
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Entities//Project.cs")`.

## Reference Dependency Interface Signatures:
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//Repositories//IProjectRepository.cs")`.

## Reference Requests and Responses:
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//AppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//AppResponse.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//CreateProjectAppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//CreateProjectAppResponse.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//RetrieveFullProjectAppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//RetrieveFullProjectAppResponse.cs")`.

## Output
Only output full source code of **ProjectAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.