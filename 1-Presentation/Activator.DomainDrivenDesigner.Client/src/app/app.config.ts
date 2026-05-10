import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ApplicationConfig, InjectionToken, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter, withEnabledBlockingInitialNavigation, withInMemoryScrolling } from '@angular/router';
import Aura from '@primeuix/themes/aura';
import { ConfirmationService } from 'primeng/api';
import { providePrimeNG } from 'primeng/config';
import { AuthService } from '../services/auth.service';
import { QueryStringService } from '../services/shared.QueryString.service';
import { StatusMessageService } from '../services/statusmessage.service';
import { routes } from './app.routes';
import { AuthFailureInterceptor } from './auth-failure.interceptor';
import { AuthInterceptor } from './auth.interceptor';
import { LoginService } from './pages/login/login.service';
import { UserClaimService } from './pages/user-claim/user-claim.service';
import { UserListGuard } from './pages/user-list/user-list.guard';
import { UserListService } from './pages/user-list/user-list.service';
import { UserListCommandService } from './pages/user-list-cmd/user-list-cmd.service';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withInMemoryScrolling({ anchorScrolling: 'enabled', scrollPositionRestoration: 'enabled' }), withEnabledBlockingInitialNavigation()),
    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } } }),
    { provide: BASE_URL, useValue: document.getElementsByTagName('base')[0].href },
    { provide: UserListGuard },
    { provide: UserClaimService },
    { provide: UserListService },
    { provide: UserListCommandService },
    { provide: LoginService },
    { provide: AuthService },
    { provide: StatusMessageService },
    { provide: QueryStringService },
    { provide: ConfirmationService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthFailureInterceptor, multi: true },
    provideHttpClient(withInterceptorsFromDi())
  ]
};
