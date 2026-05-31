import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";

@Injectable({
  providedIn: "root"
})
export class ProjectCommandService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

}
