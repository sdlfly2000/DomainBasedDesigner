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
