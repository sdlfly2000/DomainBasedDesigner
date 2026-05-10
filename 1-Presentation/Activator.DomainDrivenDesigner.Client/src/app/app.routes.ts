import { Routes } from '@angular/router';
import { AppLayout } from './layout/component/app.layout';
import { UserClaimComponent } from './pages/user-claim/user-claim.component';
import { UserListComponent } from './pages/user-list/user-list.component';
import { UserListGuard } from './pages/user-list/user-list.guard';
import { LoginComponent } from './pages/login/login.component';
import { UserRegisterComponent } from './pages/user-register/user-register.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: UserRegisterComponent },
  {
    path: 'app',
    component: AppLayout,
    children: [
      { path: 'claim', component: UserClaimComponent },
      { path: 'list', component: UserListComponent, canActivate: [UserListGuard]}
    ]
  },
  //{ path: '**', redirectTo: '/notfound' }
];
