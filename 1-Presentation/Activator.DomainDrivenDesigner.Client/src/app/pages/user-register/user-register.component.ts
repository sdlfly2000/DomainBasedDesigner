import { Component } from "@angular/core";
import { RegisterUserRequest } from "./models/RegisterUserRequest";
import { UserRegisterService } from "./user-register.service";
import { Router, RouterLink } from "@angular/router";
import { EnumInfoSeverity, StatusMessageService, StatusMessageModel } from "../../../services/statusmessage.service";
import { HttpErrorResponse } from "@angular/common/http";
import { ButtonModule } from "primeng/button";
import { DividerModule } from "primeng/divider";
import { FloatLabelModule } from "primeng/floatlabel";
import { InputTextModule } from "primeng/inputtext";
import { FormsModule } from "@angular/forms";


@Component({
  selector: 'app-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
  imports: [FormsModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule, RouterLink]
})
export class UserRegisterComponent {
  title = "User Register";

  registerUserRequest: RegisterUserRequest = {
    UserName: "",
    PasswordEncrypto: "",
    DisplayName: ""
  };

  isLoading: boolean = false;

  constructor(
    private userRegisterService: UserRegisterService,
    private router: Router,
    private statusMessageService: StatusMessageService) {

  }

  OnSubmit() {
    this.isLoading = true;
    this.userRegisterService.Register(this.registerUserRequest).subscribe({
      next: () => this.router.navigate(["/"]),
      error: errResponse => {
        if (errResponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errResponse.message, EnumInfoSeverity.Error);
        }
        this.isLoading = false;
      }
    });
  }
}
