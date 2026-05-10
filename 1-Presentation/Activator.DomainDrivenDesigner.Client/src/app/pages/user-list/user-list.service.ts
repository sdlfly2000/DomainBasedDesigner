import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BASE_URL } from "../../app.config";
import { UserModel } from "./models/UserModel";

@Injectable({
  providedIn: "root"
})
export class UserListService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {

  }

  public GetAllUsers(): Observable<UserModel[]> {
    return this.httpClient.get<UserModel[]>(this.BaseUrl + "api/UserManager/Users");
  }

  public AssignApp(userId: string, appName: string): Observable<string> {
    return this.httpClient.get<string>(this.BaseUrl + "api/UserManager/AppAssign?userId=" + userId + "&appName=" + appName);
  }
}
