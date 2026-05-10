import { HttpClient } from "@angular/common/http";
import { Injectable, Inject} from "@angular/core";
import { Observable, Subject, filter } from "rxjs";
import { BASE_URL } from "../app/app.config";

@Injectable({
  providedIn: "root"
})
export class AuthService {

  private displayNameSubject: Subject<string>;
  private isLogin: Subject<boolean>;
  private returnUrl: string | null;
 
  constructor(private httpClient: HttpClient, @Inject(BASE_URL) private baseUrl: string) {
    this.displayNameSubject = new Subject<string>();
    this.isLogin = new Subject<boolean>();
    this.returnUrl = null;
  }

  // Token and JWT
  get JwtToken() : string | null {
    return localStorage.getItem("AuthJwt");
  }

  set JwtToken(value: string) {
    localStorage.setItem("AuthJwt", value);
  }

  RemoveLocalJwt() {
    localStorage.removeItem("AuthJwt");
  }

  // Token and JWT
  get UserId() : string | null{
    return localStorage.getItem("UserID");
  }

  set UserId(value: string) {
    localStorage.setItem("UserID", value);
  }

  RemoveLocalUserId() {
    localStorage.removeItem("UserID");
  }

  get OnUserDisplayName(): Observable<string> {
    return this.displayNameSubject;
  }

  get UserDisplayName(): string | null {
    return localStorage.getItem("UserDisplayName");
  }

  set UserDisplayName(value: string) {
    localStorage.setItem("UserDisplayName", value);
    this.displayNameSubject.next(value);
  }


  // Events
  get OnLoginSuccess(): Observable<boolean> {
    return this.isLogin.pipe(
      filter(SuccessLogin => SuccessLogin));
  }

  get OnLoginFailure(): Observable<boolean> {
    return this.isLogin.pipe(
      filter(SuccessLogin => !SuccessLogin));
  }

  // Return Url
  SetReturnUrl(url: string | null) {
    this.returnUrl = url;
  }

  RemoveReturnUrl() {
    this.returnUrl = null;
  }

  get ReturnUrl(): string | null {
    return this.returnUrl;
  }

  get IsOutSideRequest() : boolean {
    return this.returnUrl != null ? true : false;
  }

  set LoginStatus(value: boolean) {
    this.isLogin.next(value);
  }

  // Login status
  get IsValidLogin(): boolean {
    if (this.JwtToken != null && this.UserDisplayName != null && this.UserId != null) {
      return true;
    }

    return false;
  }

  CheckLoginStatus() {
    if (this.IsValidLogin == true) {
      this.LoginStatus = true;
    } else {
      this.LoginStatus = false;
    }
  }

  // Check Rights
  IsAuthorizedTo(rightNames: string[]): Observable<boolean> {
    return this.httpClient.get<boolean>(this.baseUrl + "api/UserManager/Rights", {
      params: {
        rights: rightNames
      }
    });
  }

  // Clean up
  CleanLocalCache() {
    this.RemoveLocalJwt();
    this.RemoveLocalUserDisplayName();
    this.RemoveLocalUserId();
  }

  RemoveLocalUserDisplayName() {
    localStorage.removeItem("UserDisplayName");
    this.displayNameSubject.next("");
  }
}
