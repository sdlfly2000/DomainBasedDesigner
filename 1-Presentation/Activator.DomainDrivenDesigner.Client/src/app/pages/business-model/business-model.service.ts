import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { BusinessModelModel } from "./model/business-model-models";

@Injectable({
  providedIn: "root"
})
export class BusinessModelService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

    public GetAllBusinessModelsByProject(projectId: string): Observable<BusinessModelModel[]> {
      return this.httpClient.get<BusinessModelModel[]>(this.BaseUrl + "api/BusinessModel/retrieve/" + projectId, { headers: this.httpHeaders });
  }
}
