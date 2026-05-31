import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { BASE_URL } from './app.config';
import { Environment } from '../environments/environment';

@Injectable()
export class AuthFailureInterceptor implements HttpInterceptor {

  constructor(private router: Router, private authService: AuthService, @Inject(BASE_URL) private BaseUrl: string) { }

  intercept(req: HttpRequest<unknown>, next: HttpHandler) {

    return next.handle(req)
      .pipe(
        tap({
          error: (err) => {
            if (err instanceof HttpErrorResponse && err.status == 401) {
              this.authService.CleanLocalCache();
              window.location.href = Environment.AuthServiceBaseUrl + "#/login?returnUrl=" + this.BaseUrl;            
            }
          }
        })
      );
  }
}
