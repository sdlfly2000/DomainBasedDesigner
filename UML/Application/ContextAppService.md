# Activator.DomainDrivenDesigner.Application.Services.ContextAppService
```mermaid
graph TB
    subgraph RetrieveContexts[RetrieveContexts]
        direction TB
        start(("Start"s)) -->
        |request: RetrieveContextAppRequest| loadAllContext["Load All Context -> IDDDRepository.RetrieveContexts()"] -->
        return["`Return **Context**`"]
    end

    subgraph main CreateContext[CreateContext]
        direction TB
        start2(("Start")) --> 
        |request: CreateContextAppRequest| CreateContext["Create a Context -> IDDDRepository.CreateContexts(request.Name, request.projectId)"]-->
        return2["`Return **ContextId**`"]            
    end
```
---

```mermaid
classDiagram
    class AppRequest {
        + Id: Guid
    }

    class AppResponse {
        + RequestId： Guid
        + Success: bool
        + ErrorMessage: string?
    }
    
    class CreateContextAppRequest {
        + Name: string
        + ProjectId: Guid
    }

    class CreateContextAppResponse {
        + ContextId: Guid
    }

    class RetrieveContextAppRequest {

    }

    class RetrieveContextAppResponse {
        + Contexts: List~Context~
    }

    %% Relationship
    AppRequest <|-- RetrieveContextAppRequest
    AppResponse <|-- RetrieveContextAppResponse

    AppRequest <|-- CreateContextAppRequest
    AppResponse <|-- CreateContextAppResponse

```