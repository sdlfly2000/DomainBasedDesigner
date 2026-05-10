import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, WritableSignal, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { Select } from 'primeng/select';
import { AuthService } from '../../../services/auth.service';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { ClaimTypeValues } from './models/ClaimTypeValues';
import { UserClaim } from './models/UserClaim';
import { UserClaimService } from './user-claim.service';

@Component({
  selector: 'app-root',
  templateUrl: './user-claim.component.html',
  styleUrls: ['./user-claim.component.css'],
  imports: [FormsModule, ConfirmDialogModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule, Dialog, Select ]
})
export class UserClaimComponent implements OnInit{
  title = 'User Claims';
  UserId: string | null;
  UserClaims: WritableSignal<UserClaim[]> = signal<UserClaim[]>([]);
  ClaimTypes: WritableSignal<ClaimTypeValues[]> = signal<ClaimTypeValues[]>([]);

  isPopupAddClaimDialog: boolean = false;
  isPopupUpdateClaimDialog: boolean = false;

  UserClaimSelected: UserClaim = {
    claimType: {
      typeShortName: '',
      typeName: ''
    },
    claimId: '',
    value: '',
    isFixed: false
  };
  
  NewUserClaim: UserClaim = {
    claimType: {
        typeShortName: '',
        typeName: ''
    },
    claimId: '',
    value: "",
    isFixed: false
  };  

  constructor(
      private route: ActivatedRoute,
      private router: Router,
      private userClaimService: UserClaimService,
      private statusMessageService: StatusMessageService,
      private authService: AuthService) {
    this.UserId = this.route.snapshot.queryParamMap.get("userid") ?? this.authService.UserId;
  }

  ngOnInit(): void {
    this.userClaimService.GetUserClaims(this.UserId!).subscribe(claims => this.UserClaims.set(claims));    
    this.userClaimService.GetAllClaimTypes().subscribe(types => this.ClaimTypes.set(types));
  }

  UpdateSelected(userClaim: UserClaim): void {
    this.UserClaimSelected = this.Clone(userClaim);
    this.ShowUpdateClaimDialog(true);
  }

  DeleteClaim(userClaim: UserClaim): void {
      this.userClaimService.DeleteUserClaim(this.UserId!, userClaim).subscribe({
      complete: () => {
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully delete a Claim", EnumInfoSeverity.Info);
      }
    });
  }

  UpdateClaim(): void {
    this.userClaimService.UpdateUserClaim(this.UserId!, this.UserClaimSelected).subscribe({
      complete: () => {
        this.ShowUpdateClaimDialog(false);
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully update a Claim", EnumInfoSeverity.Info);
      }
    });
  }

  AddClaim(): void {
    this.userClaimService.AddUserClaim(this.UserId!, this.NewUserClaim).subscribe({
      complete: () => {
        this.ShowAddClaimDialog(false);
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.error.detail, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully add a Claim", EnumInfoSeverity.Info);
      }
    });
  }

  ShowAddClaimDialog(isShow: boolean){
    this.isPopupAddClaimDialog = isShow;
    if (!isShow) {
      this.NewUserClaim = {
        claimType: {
          typeShortName: '',
          typeName: ''
        },
        claimId: '',
        value: "",
        isFixed: false
      };
    }
  }

  ShowUpdateClaimDialog(isShow: boolean) {
    this.isPopupUpdateClaimDialog = isShow;
    if (!isShow) {
      this.UserClaimSelected = {
        claimType: {
          typeShortName: '',
          typeName: ''
        },
        claimId: '',
        value: "",
        isFixed: false
      };
    }
  }

  private Clone(userClaim: UserClaim): UserClaim {
    return {
      claimType: {
        typeShortName: userClaim.claimType.typeShortName,
        typeName: userClaim.claimType.typeName
      },
      claimId: userClaim.claimId,
      value: userClaim.value,
      isFixed: userClaim.isFixed
    };  
  }
}
