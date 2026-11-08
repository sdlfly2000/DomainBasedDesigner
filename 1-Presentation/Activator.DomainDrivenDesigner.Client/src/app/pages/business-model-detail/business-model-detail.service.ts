import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { BASE_URL } from "../../app.config";
import { BusinessModelModel } from "../business-model/model/business-model-models";

@Injectable({
  providedIn: "root"
})
export class BusinessModelDetailService {
    private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

    constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

    public GetBusinessModelsById(businessModelId: string | undefined): Observable<BusinessModelModel> {
        if (businessModelId == undefined) {
            return throwError(() => new Error("Business model ID is undefined"));
        }

        return this.httpClient.get<BusinessModelModel>(this.BaseUrl + "api/BusinessModel/retrieve/id/" + businessModelId, { headers: this.httpHeaders });
    }
}
