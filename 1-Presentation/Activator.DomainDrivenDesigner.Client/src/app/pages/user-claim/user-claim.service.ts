import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserClaim } from "./models/UserClaim";
import { ClaimTypeValues } from "./models/ClaimTypeValues";
import { BASE_URL } from "../../app.config";

@Injectable({
  providedIn: "root"
})
export class UserClaimService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private BaseUrl: string) {

  }

  GetUserClaims(UserID: string): Observable<UserClaim[]> {
    return this.httpClient.get<UserClaim[]>(this.BaseUrl + "api/ClaimManager/GetClaimByUserId?id=" + UserID);
  }

  UpdateUserClaim(UserId: string, UserClaim: UserClaim): Observable<string> {
    return this.httpClient.post<string>(this.BaseUrl + "api/ClaimManager/UpdateUserClaim?id=" + UserId, JSON.stringify(UserClaim), { headers: this.httpHeaders });
  }

  DeleteUserClaim(UserId: string, UserClaim: UserClaim): Observable<string> {
    return this.httpClient.post<string>(this.BaseUrl + "api/ClaimManager/DeleteUserClaim?id=" + UserId, JSON.stringify(UserClaim), { headers: this.httpHeaders });
  }

  AddUserClaim(UserId: string, UserClaim: UserClaim): Observable<string> {
    return this.httpClient.post<string>(this.BaseUrl + "api/ClaimManager/AddUserClaim?id=" + UserId, JSON.stringify(UserClaim), { headers: this.httpHeaders });
  }

  GetAllClaimTypes(): Observable<ClaimTypeValues[]> {
    return this.httpClient.get<ClaimTypeValues[]>(this.BaseUrl + "api/ClaimManager/GetClaimTypes");
  }
}
