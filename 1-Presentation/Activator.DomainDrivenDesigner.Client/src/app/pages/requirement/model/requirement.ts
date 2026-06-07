export interface RequirementModel {
    id: string;
    description: string;
    createdOnUTC: Date;
}

export interface RetrieveRequirementByProjectAppResponseModel {
    requirements: RequirementModel[];
    success: boolean;
    errorMessage: string;
}
