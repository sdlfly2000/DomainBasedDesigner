## Generate complete C# code 
Target File: **2-Application/Activator.DomainDrivenDesigner.Application/Services/BusinessModelAppService.cs**

## Format:
- **Formatting Style:** Strictly use Allman style (opening braces `{` must always be placed on a new line for classes, methods, and control blocks).

- Write a C# **BusinessModelAppService** class

    ```csharp
    namespace Activator.DomainDrivenDesigner.Application.Services;

    public class BusinessModelAppService
    {
        // Your Full Code Implementation (Allman Style including Using statements)
    }
    ```

## Rules:
1. Class: **BusinessModelAppService**, Implements: **IBusinessModelAppService**

2. Namespace in file-scope: `namespace Activator.DomainDrivenDesigner.Application.Services;`

3. Inject below through constructor
- **IBusinessModelRepository** (store in a private readonly field `_businessModelRepository`)
- **IBusinessModelPersistor** (store in a private readonly field `_businessModelPersistor`)
- **IServiceProvider**, not using (store in a private readonly field `_serviceProvider`)

4. Place Attributes
- Decorate **BusinessModelAppService** class with [ServiceLocate(typeof(BusinessModelAppService))].
- Decorate each public method with `[LogTrace(typeof({ResponseTypeName}))]`, replacing `{ResponseTypeName}` with the corresponding concrete return type. Ensure the `[LogTrace]` attribute target type references the underlying response record, *not* the wrapping `Task<>` type.

5. Public async method signature: `Task<RetrieveBusinessModelByIdAppResponse> RetrieveBusinessModelById(RetrieveBusinessModelByIdAppRequest request)`

    Method **RetrieveBusinessModelById** logic: 
    Execute `read_action_md_file("UML//Application//BusinessModelAppService.md","RetrieveBusinessModelById")`.

5. Public async method signature: `Task<UpsertBusinessModelsAppResponse> UpsertProjectBusinessModels(UpsertBusinessModelsAppRequest request)`

    Method **UpsertProjectBusinessModels** logic: 
    Execute `read_action_md_file("UML//Application//BusinessModelAppService.md","UpsertProjectBusinessModels")`.

5. Public async method signature: `Task<RetrieveBusinessModelByNameAppResponse> RetrieveBusinessModelByName(RetrieveBusinessModelsByNameAppRequest request)`

    Method **RetrieveBusinessModelByName** logic: 
    Execute `read_action_md_file("UML//Application//BusinessModelAppService.md","RetrieveBusinessModelByName")`.

## Context Boundaries:
- **Ignore Exception Handling:** Omit manual try-catch wrappers since exceptions are decoupled via the infrastructure `LogTrace` attribute tier.
- **Asynchronous Execution:** Every data tier interaction must map via explicit asynchronous operations utilizing `ConfigureAwait(false)`.

## Reference Domain Models:
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//BusinessModel//Entities//BusinessModel.cs")`.
- Execute `read_code_file("5-Support//Activator.DomainDrivenDesigner.Support.Core//Marks//EntityBase.cs")`.

## Reference Dependency Interface Signatures:
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//BusinessModel//IBusinessModelRepository.cs")`.
- Execute `read_code_file("3-Domain//Activator.DomainDrivenDesigner.Domain//BusinessModel//IBusinessModelPersistor.cs")`.

## Reference Requests and Responses:
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//AppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//AppResponse.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//UpsertBusinessModelsAppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//UpsertBusinessModelsAppResponse.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//RetrieveBusinessModelsByNameAppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//RetrieveBusinessModelByNameAppResponse.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppRequests//RetrieveBusinessModelByIdAppRequest.cs")`.
- Execute `read_code_file("2-Application//Activator.DomainDrivenDesigner.Application//AppResponses//RetrieveBusinessModelByIdAppResponse.cs")`.

## Output
Only output full source code of **BusinessModelAppService.cs** in C# format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.