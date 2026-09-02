export interface RequirementDetailModel {
    requirementId: string;
    description: string;
}

export interface Context {
    id: string
    name: string
    projectId: string
}

export interface CreateContextRequest {
    name: string
    projectId: string
}
