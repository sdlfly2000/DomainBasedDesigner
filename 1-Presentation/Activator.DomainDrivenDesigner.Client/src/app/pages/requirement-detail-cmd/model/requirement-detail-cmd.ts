export interface AnalyzeRequirementsRequestModel {
    description: string;
}

export interface AnalyzeRequirementsResponseModel {
    businessModels: BusinessModel[]
    raw: string;
}

export interface BusinessModel {
    id: string | undefined
    name: string | undefined
    contentMermaid: string | undefined
    contextId: string | undefined
    contextName: string | undefined
    createdOnUtc: Date | undefined
}

export class BusinessModelInstance implements BusinessModel {
    id: string | undefined;
    name: string | undefined;
    contentMermaid: string | undefined;
    contextId: string | undefined;
    contextName: string | undefined;
    createdOnUtc: Date | undefined;

    constructor(id?: string, name?: string, contentMermaid?: string, contextId?: string, contextName?: string, createdOnUtc?: Date) {
        this.id = id;
        this.name = name;
        this.contentMermaid = contentMermaid;
        this.contextId = contextId;
        this.contextName = contextName;
        this.createdOnUtc = createdOnUtc;
    }

    public static NotNullValidate(model: BusinessModel): boolean {
        return model.contextId !== undefined &&
               model.contextName !== undefined &&
               model.name !== undefined;
    }
}

// Save Requirement Request and Response Models
export interface SaveRequirementRequestModel {
    projectId: string | undefined
    requirementId: string | undefined
    description: string | undefined
}

export interface SaveRequirementResponseModel {
    requestId: string
    success: boolean
    errorMessage: string
}

