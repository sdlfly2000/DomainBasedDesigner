export interface AnalyzeRequirementsRequestModel {
    description: string;
}

export interface AnalyzeRequirementsResponseModel {
    businessModels: BusinessModel[]
    raw: string;
}

export interface BusinessModel {
    id: string
    name: string
    rawDescription: string
    properties: BusinessModelProperty[]
}

export interface BusinessModelProperty {
    name?: string
    Type?: ModelPropertyType
}

export enum ModelPropertyType {
    None = 0,
    String = 1,
    Int = 2,
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

