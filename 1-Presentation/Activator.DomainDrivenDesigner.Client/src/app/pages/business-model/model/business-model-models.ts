export interface BusinessModelModel {
    id: string;
    name: string;
    contentMermaid: string;
    contextId: string;
    createdOnUtc: Date;
}

export const CreateDefaultBusinessModel = (): BusinessModelModel => ({
    id: "",
    name: "",
    contentMermaid: "",
    contextId: "",
    createdOnUtc: new Date()
});
