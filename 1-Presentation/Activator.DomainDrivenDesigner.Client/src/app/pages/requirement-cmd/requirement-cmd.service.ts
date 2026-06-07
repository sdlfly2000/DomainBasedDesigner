import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { CreateRequirementAppRequest } from "./model/requirement-cmd";

@Injectable({
  providedIn: "root"
})
export class RequirementCommandService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

  public NewRequirement(request:CreateRequirementAppRequest): Observable<string> {
      return this.httpClient.post<string>(this.BaseUrl + "api/requirement/create", request, { headers: this.httpHeaders });
  }
}
