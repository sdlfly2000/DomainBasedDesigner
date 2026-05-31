import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DividerModule } from 'primeng/divider';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { ProjectService } from './project.service';
import { ProjectModel } from './model/project';

@Component({
  selector: 'app-root',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css'],
  imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, DividerModule, ToolbarModule]
})
export class ProjectComponent implements OnInit{
    title = 'Projects';
    IsLoading: boolean = true;

    Projects: WritableSignal<ProjectModel[]> = signal<ProjectModel[]>([]);

    constructor(
        private projectService: ProjectService,
        private statusMessageService: StatusMessageService) {
    }

    ngOnInit(): void {
        this.projectService.GetAllProjects().subscribe({
            next: (response) => {
                this.Projects.set(response.Projects);
                this.IsLoading = false;
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            }
        });
    }
}
