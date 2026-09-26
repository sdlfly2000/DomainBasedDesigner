# Activator.DomainDrivenDesigner.Application.Services

```mermaid method:"RetrieveContexts"
graph TB
    subgraph RetrieveContexts
        direction TB
        start(("Start")) -->
        |request: RetrieveContextAppRequest| RetrieveContexts["`Load All **Context**s domain model by request.ProjectId`"] -->
        return["`Return RetrieveContextAppResponse with loaded **Context**s`"]
    end
```

```mermaid method:"CreateContext"
graph TB
    subgraph CreateContext
        direction TB
        start2(("Start")) --> 
        |request: CreateContextAppRequest| CreateContext["`New a **Context** domain model with Name and ProjectId from request`"] -->
        return2["`Return CreateContextAppResponse with **ContextId**`"]
    end
```

```mermaid method:"UpdateContext"
graph TB
    subgraph UpdateContext
        direction TB
        start2(("Start")) --> 
        |request: UpdateContextAppRequest| updateContext["`Update **Context** domain model with Name from request`"] -->
        return2["`Return UpdateContextAppResponse`"]
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

    class UpdateContextAppRequest {
        + ContextId: Guid,
        + Name: string
    }

    class UpdateContextAppResponse {
        + ContextId: Guid
    }

    %% Relationship
    AppRequest <|-- RetrieveContextAppRequest
    AppResponse <|-- RetrieveContextAppResponse

    AppRequest <|-- CreateContextAppRequest
    AppResponse <|-- CreateContextAppResponse

    AppRequest <|-- UpdateContextAppRequest
    AppResponse <|-- UpdateContextAppResponse

```