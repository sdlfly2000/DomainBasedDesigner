import { Routes } from '@angular/router';
import { AppLayout } from './layout/component/app.layout';
import { UserListComponent } from './pages/user-list/user-list.component';
import { UserListGuard } from './pages/user-list/user-list.guard';
import { LandingComponent } from './pages/landing/landing.component';
import { ProjectComponent } from './pages/project/project.component';
import { RequirementComponent } from './pages/requirement/requirement.component';

export const routes: Routes = [
  { path: "", component: LandingComponent },
  {
    path: 'app',
    component: AppLayout,
    children: [
      { path: 'project', component: ProjectComponent },
      { path: 'requirement', component: RequirementComponent },
      { path: 'list', component: UserListComponent, canActivate: [UserListGuard]}
    ]
  },
  //{ path: '**', redirectTo: '/notfound' }
];
