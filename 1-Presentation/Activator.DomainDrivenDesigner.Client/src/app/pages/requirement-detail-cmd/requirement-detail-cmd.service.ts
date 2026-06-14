import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { AnalyzeRequirementsRequestModel, AnalyzeRequirementsResponseModel } from "./model/requirement-detail-cmd";

@Injectable({
  providedIn: "root"
})
export class RequirementDetailCommandService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

  public AnalyzeRequirement(request: AnalyzeRequirementsRequestModel): Observable<AnalyzeRequirementsResponseModel> {
      return this.httpClient.post<AnalyzeRequirementsResponseModel>(this.BaseUrl + "api/requirement/analyze", request, { headers: this.httpHeaders });
  }
}
