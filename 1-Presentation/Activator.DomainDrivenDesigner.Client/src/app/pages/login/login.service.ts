import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LoginRequest } from "./models/LoginRequest";
import { LoginResponse } from "./models/LoginResponse";
import { AuthService } from "../../../services/auth.service";
import { BASE_URL } from "../../app.config";

@Injectable({
  providedIn: "root"
})
export class LoginService{

  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json" , "Access-Control-Allow-Origin":"*"});

  constructor(private httpClient: HttpClient, private authService: AuthService, @Inject(BASE_URL) private baseUrl: string) { }

  public Authenticate(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(this.baseUrl + "api/Authentication/Authenticate", JSON.stringify(request), { headers: this.httpHeaders });
  }
}
