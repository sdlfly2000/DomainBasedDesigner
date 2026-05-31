import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { AuthService } from '../../../services/auth.service';
import { ConfirmationService } from 'primeng/api';
import { FormsModule } from '@angular/forms';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { Environment } from '../../../environments/environment';
import { BASE_URL } from '../../app.config';

@Component({
  selector: 'app-login',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
  imports: [FormsModule, ConfirmDialogModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule],
  providers: [ConfirmationService]
})
export class LandingComponent implements OnInit {
  title = 'Landing';

  constructor(
    private router: Router,
    private authService: AuthService,
    private queryStringService: QueryStringService,
    @Inject(BASE_URL) private BaseUrl: string
  ) {

  }

  ngOnInit(): void {
      let jwtToken = this.queryStringService.Get("jwtToken");
      let displayName = this.queryStringService.Get("userDisplayName");
      let userId = this.queryStringService.Get("userid");

      if (jwtToken != null) this.authService.JwtToken = jwtToken;
      if (displayName != null) this.authService.UserDisplayName = displayName;
      if (userId != null) this.authService.UserId = userId;

      if (!this.authService.IsValidLogin) {
          this.authService.CleanLocalCache();
          window.location.href = Environment.AuthServiceBaseUrl + "#/login?returnUrl=" + this.BaseUrl;
      }

      this.router.navigate(["app"]);
  }
}
