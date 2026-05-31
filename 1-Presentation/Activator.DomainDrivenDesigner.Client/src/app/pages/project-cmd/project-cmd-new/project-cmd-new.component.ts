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
import { ProjectCommandService } from '../project-cmd.service';

@Component({
  standalone: true,
  selector: 'project-cmd-new',
  templateUrl: './project-cmd-new.component.html',
  styleUrls: ['./project-cmd-new.component.css'],
  imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, Dialog]
})
export class ProjectCommandNewComponent {

    constructor(
        private projectCommandService: ProjectCommandService,
        private statusMessageService: StatusMessageService) {
    }

    
}
