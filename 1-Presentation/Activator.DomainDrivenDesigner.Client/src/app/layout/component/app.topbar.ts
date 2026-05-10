import { Component } from '@angular/core';
import { RouterModule, Router} from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '../service/layout.service';
import { AppTopBarService } from '../service/app.topbar.service';
import { AuthService } from '../../../services/auth.service';
import { StatusMessageService } from '../../../services/statusmessage.service';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
    selector: 'app-topbar',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule, ConfirmDialogModule],
    template: `<div class="layout-topbar">
        <div class="layout-topbar-logo-container">
            <button class="layout-menu-button layout-topbar-action" (click)="layoutService.onMenuToggle()">
                <i class="pi pi-bars"></i>
            </button>
            <a class="layout-topbar-logo" routerLink="/">
                <span>Auth Service</span>
            </a>
        </div>

        <div class="layout-topbar-actions">
           
            <button class="layout-topbar-menu-button layout-topbar-action" pStyleClass="@next" enterFromClass="hidden" enterActiveClass="animate-scalein" leaveToClass="hidden" leaveActiveClass="animate-fadeout" [hideOnOutsideClick]="true">
                <i class="pi pi-ellipsis-v"></i>
            </button>

            <div class="layout-topbar-menu hidden lg:block">
                <div class="layout-topbar-menu-content">
                    @if(isSignIn) {
                        <button type="button" class="layout-topbar-action" (click)="Logout()">
                            <i class="pi pi-sign-out"></i>
                            <span>Sign Out</span>
                        </button>
                        <button type="button" class="layout-topbar-action">
                            <i class="pi pi-user"></i>
                            <span>{{displayName}}</span>
                        </button>
                    } @else {
                        <button type="button" class="layout-topbar-action">
                            <i class="pi pi-user-plus"></i>
                            <span>Register</span>
                        </button>
                    }                 
                </div>
            </div>
        </div>
    </div>`
})
export class AppTopbar {
    displayName: string | null = null;

    isSignIn: boolean = false;
    
    constructor(
        public layoutService: LayoutService, 
        private appTopBarService: AppTopBarService,
        private authService: AuthService,
        private statusMessageService: StatusMessageService,
        private router: Router
    ) {
        this.authService.OnUserDisplayName.subscribe(name => this.displayName = name);
        this.displayName = this.authService.UserDisplayName

        this.authService.OnLoginSuccess.subscribe(() => {
        this.isSignIn = true;
        });

        this.authService.OnLoginFailure.subscribe(() => {
        this.isSignIn = false;
        })

        this.authService.CheckLoginStatus();
    }
   
    Logout() {
        this.appTopBarService.logout().subscribe(() =>
        {
            this.authService.CleanLocalCache();
            this.authService.LoginStatus = false;
            this.router.navigate(["/"]);
        });
    }
}
