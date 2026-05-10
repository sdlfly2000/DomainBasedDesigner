import { HttpErrorResponse } from '@angular/common/http';
import { Component, input, OnChanges, WritableSignal, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { Select } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../../services/statusmessage.service';
import { UserModel } from '../../user-list/models/UserModel';
import { UserListCommandService } from '../user-list-cmd.service';

@Component({
    standalone: true,
    selector: 'user-list-cmd-assign-role',
    templateUrl: './user-list-cmd-assign-role.component.html',
    styleUrls: ['./user-list-cmd-assign-role.component.css'],
    imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, Dialog, Select]
})
export class UserListCommandAssignRoleComponent implements OnChanges {
    isPopupAssignRoleDialog: boolean = false;
    assignAppName: string = '';
    assignRoleName: string = '';
    selectedUsers = input<UserModel[]>();
    disabledCommand: boolean = !this.selectedUsers || !this.selectedUsers.length;
    appNames: WritableSignal<string[]> = signal<string[]>([]);

    constructor(
        private userListCommandService: UserListCommandService,
        private statusMessageService: StatusMessageService) {
        this.userListCommandService.GetAppNames().subscribe({
            next: (response) => {
                this.appNames.set(response);
            },
            error: (errorResponse) => {
                if (errorResponse instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(errorResponse.error.detail, EnumInfoSeverity.Error);
                }
            }
        });
    }

    ngOnChanges() {
        this.disabledCommand = !this.selectedUsers() || !this.selectedUsers()?.length;
    }

    AssignRole(): void {
        this.selectedUsers()?.forEach(user => {
            this.userListCommandService.AssignRole(user.id.code, this.assignAppName, this.assignRoleName).subscribe({
                next: () => {
                    this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully Assign Role for " + user.userName, EnumInfoSeverity.Info);
                },
                error: (errorResponse) => {
                    if (errorResponse instanceof HttpErrorResponse) {
                        this.statusMessageService.StatusMessage = new StatusMessageModel(errorResponse.error.detail + " for " + user.userName, EnumInfoSeverity.Error);
                    }
                },
                complete: () => {
                    this.assignRoleName = '';
                    this.assignAppName = '';
                }
            });
        });

        this.ShowAssignRoleDialog(false);
    }

    ShowAssignRoleDialog(isShow: boolean) {
        this.isPopupAssignRoleDialog = isShow;
        if (!isShow) {
            this.assignAppName = '';
        }
    }
}
