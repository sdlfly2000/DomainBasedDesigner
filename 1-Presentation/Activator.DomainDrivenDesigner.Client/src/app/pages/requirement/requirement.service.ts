import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { BASE_URL } from "../../app.config";
import { RetrieveRequirementByProjectAppResponseModel } from "./model/requirement";

@Injectable({
  providedIn: "root"
})
export class RequirementService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

    public GetAllRequirements(projectId: string | undefined): Observable<RetrieveRequirementByProjectAppResponseModel> {
        if (projectId == undefined) {
            return throwError(() => new Error("ProjectId is null when GetAllRequirements"));
        }

        return this.httpClient.get<RetrieveRequirementByProjectAppResponseModel>(this.BaseUrl + "api/requirement/project/" + projectId, { headers: this.httpHeaders });
  }
}
