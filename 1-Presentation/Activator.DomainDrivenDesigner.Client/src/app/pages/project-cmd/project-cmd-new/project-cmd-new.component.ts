import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../../services/statusmessage.service';
import { ProjectCommandService } from '../project-cmd.service';
import { TextareaModule } from 'primeng/textarea';
import { CreateProjectAppRequest } from '../model/project-cmd';

@Component({
  standalone: true,
  selector: 'project-cmd-new',
  templateUrl: './project-cmd-new.component.html',
  styleUrls: ['./project-cmd-new.component.css'],
    imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, Dialog, TextareaModule]
})
export class ProjectCommandNewComponent {

    isPopupNewProjectDialog: boolean = false;
    newProjectName: string = "";
    newProjectDescription: string = "";

    constructor(
        private projectCommandService: ProjectCommandService,
        private statusMessageService: StatusMessageService) {
    }

    NewProject() {

        let request: CreateProjectAppRequest = {
            name: this.newProjectName,
            description: this.newProjectDescription
        }

        this.projectCommandService.NewProject(request).subscribe({
            next: (response) => {
                this.statusMessageService.StatusMessage = new StatusMessageModel("Successful to create a Project", EnumInfoSeverity.Info);
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            },
            complete: () => {
                this.newProjectName = "";
                this.newProjectDescription = "";
                this.ShowNewProjectDialog(false);
                window.location.reload();
            }
        });
    }

    ShowNewProjectDialog(isShow: boolean) {
        this.isPopupNewProjectDialog = isShow;
        if (!isShow) {
            this.newProjectName = "";
            this.newProjectDescription = "";
        }
    }    
}
