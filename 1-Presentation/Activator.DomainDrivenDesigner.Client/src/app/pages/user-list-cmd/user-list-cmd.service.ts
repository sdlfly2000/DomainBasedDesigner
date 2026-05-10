import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";

@Injectable({
  providedIn: "root"
})
export class UserListCommandService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {}

  public AssignApp(userId: string, appName: string): Observable<string> {
    return this.httpClient.get<string>(this.BaseUrl + "api/UserManager/AppAssign?userId=" + userId + "&appName=" + appName);
  }

  public AssignRole (userId: string, appName: string, roleName: string): Observable<string> {
    return this.httpClient.get<string>(this.BaseUrl + "api/UserManager/RoleAssign?userId=" + userId + "&appName=" + appName + "&roleName=" + roleName);
  }

  public GetAppNames(): Observable<string[]> {
    return this.httpClient.get<string[]>(this.BaseUrl + "api/ClaimManager/GetAppNames");
  }
}
