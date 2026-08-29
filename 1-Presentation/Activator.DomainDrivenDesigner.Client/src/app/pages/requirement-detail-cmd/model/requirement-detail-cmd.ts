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

export interface Context {
    id: string | undefined
    name: string | undefined
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

