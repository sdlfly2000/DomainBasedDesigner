import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { BusinessModel } from "../requirement-detail-cmd/model/requirement-detail-cmd";
import { Context, CreateContextRequest, RequirementDetailModel } from "./model/requirement-detail";

@Injectable({
  providedIn: "root"
})
export class RequirementDetailService {
    private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

    constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

    public AnalyzeRequirement(description: string): Observable<string> {
        return this.httpClient.post<string>(this.BaseUrl + "api/requirement/analyze", description, { headers: this.httpHeaders });
    }

    public UpsertBusinessModel(model: BusinessModel, requirementId: string): Observable<boolean> {
        return this.httpClient.post<boolean>(this.BaseUrl + `api/businessmodel/upsert/${requirementId}`, model, { headers: this.httpHeaders });
    }

    public RetrieveBusinessModel(modelName: string, requirementId: string): Observable<BusinessModel> {
        return this.httpClient.get<BusinessModel>(this.BaseUrl + `api/businessmodel/retrieve/${requirementId}/${modelName}`, { headers: this.httpHeaders });
    }

    public RetrieveRequirement(requirementId: string): Observable<RequirementDetailModel> {
        return this.httpClient.get<RequirementDetailModel>(this.BaseUrl + `api/requirement/retrieve/${requirementId}`, { headers: this.httpHeaders });
    }

    public RetrieveContexts(projectId: string): Observable<Context[]> {
        return this.httpClient.get<Context[]>(this.BaseUrl + `api/context/retrieve/${projectId}`, { headers: this.httpHeaders });
    }

    public CreateContext(request: CreateContextRequest): Observable<string> {
        return this.httpClient.post<string>(this.BaseUrl + `api/context/create`, request, { headers: this.httpHeaders });
    }
}
