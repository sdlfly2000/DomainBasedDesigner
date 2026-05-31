export interface ProjectModel {
    Id: string;
    Name: string;
    Description: string;
}

export interface RetrieveFullProjectAppResponseModel {
    RequestId: string;
    Projects: ProjectModel[];
    Success: boolean;
    ErrorMessage: string;
}

