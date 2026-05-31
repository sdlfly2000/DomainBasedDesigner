import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { RetrieveFullProjectAppResponseModel } from "./model/project";

@Injectable({
  providedIn: "root"
})
export class ProjectService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

    public GetAllProjects(): Observable<RetrieveFullProjectAppResponseModel> {
        return this.httpClient.get<RetrieveFullProjectAppResponseModel>(this.BaseUrl + "api/project/loadall", { headers: this.httpHeaders });
    }
}
