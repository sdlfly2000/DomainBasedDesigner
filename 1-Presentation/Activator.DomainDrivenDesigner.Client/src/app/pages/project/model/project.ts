export interface ProjectModel {
    id: string;
    name: string;
    description: string;
    createdOnUTC: Date;
}

export interface RetrieveFullProjectAppResponseModel {
    requestId: string;
    projects: ProjectModel[];
    success: boolean;
    errorMessage: string;
}

