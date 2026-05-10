import { HttpErrorResponse } from '@angular/common/http';
import { Component, input, OnChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../../services/statusmessage.service';
import { UserModel } from '../../user-list/models/UserModel';
import { UserListCommandService } from '../user-list-cmd.service';

@Component({
  standalone: true,
  selector: 'user-list-cmd-assign-app',
  templateUrl: './user-list-cmd-assign-app.component.html',
  styleUrls: ['./user-list-cmd-assign-app.component.css'],
  imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, Dialog]
})
export class UserListCommandAssignAppComponent implements OnChanges {
    isPopupAssignAppDialog: boolean = false;
    assignAppName: string = '';
    selectedUsers = input<UserModel[]>();
    disabledCommand: boolean = !this.selectedUsers || !this.selectedUsers.length;

    constructor(
        private userListCommandService: UserListCommandService,
        private statusMessageService: StatusMessageService) {
    }

    ngOnChanges() {
        this.disabledCommand = !this.selectedUsers() || !this.selectedUsers()?.length;
    }

    AssignApp(): void {
        this.selectedUsers()?.forEach(user => {
            this.userListCommandService.AssignApp(user.id.code, this.assignAppName).subscribe({
                next: () => {
                    this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully Assign App", EnumInfoSeverity.Info);
                },
                error: (errorResponse) => {
                    if (errorResponse instanceof HttpErrorResponse) {
                        this.statusMessageService.StatusMessage = new StatusMessageModel(errorResponse.error.detail, EnumInfoSeverity.Error);
                    }
                }
            });
        });

        this.ShowAssignAppDialog(false);
    }

    ShowAssignAppDialog(isShow: boolean) {
        this.isPopupAssignAppDialog = isShow;
        if (!isShow) {
            this.assignAppName = '';
        }
    }
}
