import { BASE_URL } from "@/app.config";
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AppFooterService {
  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private baseUrl: string) {
  }

  public GetVersion(): Observable<string> {
      return this.httpClient.get(this.baseUrl + "api/Version", {responseType: "text"})
  }
}
