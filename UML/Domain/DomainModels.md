# Domain

```mermaid
classDiagram
namespace nsProject["Domain.Project"] {
    class Project {
        <<AggregateRoot>>
        %% inherited
        + ID: Guid
        %% inherited
        + CreatedOnUtc: Datetime

        + Name: String
        + Description: String?
        + Requirements: List~Requirement~
        + ContextIds: List~Guid~
        + BaseDirectory: String?

        + Create(ProjectName:string, ProjectDescription:string, BaseDirectory: string = null) : Project
    }

    class Requirement {
        <<Entity>>
        %% inherited
        + ID: Guid
        %% inherited
        + CreatedOnUtc: Datetime

        + Description: String
        + BusinessActionIds: List~Guid~
        + BusinessModelIds: List~Guid~
    }
}

namespace nsBusinessAction["Domain.BusinessAction"] {
    class BusinessAction {
        <<AggregateRoot>>
        %% inherited
        + ID: Guid
        %% inherited
        + CreatedOnUtc: Datetime
        
        + Name: String?
        + ContextId: Guid
        + ContentMermaid: String?
    }
}

namespace nsBusinessModel["Domain.BusinessModel"] {
    class BusinessModel {
        <<AggregateRoot>>
        %% inherited
        + ID: Guid
        %% inherited
        + CreatedOnUtc: Datetime 
        
        + Name: String?
        + ContentMermaid: String?
        + ContextId: Guid
    }
}

namespace nsContext["Domain.Context"] {
    class Context {
        <<AggregateRoot>>
        %% inherited
        + ID: Guid
        %% inherited
        + CreatedOnUtc: Datetime

        + Name: String
    }
}

%% Entity Relationship

Project "1" --> "0..*" Requirement
Project "1" ..> "0..n" Context

Requirement "1" ..> BusinessModel : 0..*
Requirement "1" ..> BusinessAction : 0..*

BusinessModel "0..n" ..> "1" Context
BusinessAction "0..n" ..> "1" Context

```