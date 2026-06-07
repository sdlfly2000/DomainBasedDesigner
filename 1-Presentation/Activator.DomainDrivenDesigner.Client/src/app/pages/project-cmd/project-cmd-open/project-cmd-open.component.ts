import { Component, input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { Router } from '@angular/router';
import { ProjectModel } from '../../project/model/project';

@Component({
  standalone: true,
  selector: 'project-cmd-open',
  templateUrl: './project-cmd-open.component.html',
  styleUrls: ['./project-cmd-open.component.css'],
    imports: [ButtonModule]
})
export class ProjectCommandOpenComponent implements OnChanges{
    projectId: string = "";
    selectedProjects = input<ProjectModel[]>();
    disabledCommand: boolean = true;

    constructor(
        private router: Router) {
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.disabledCommand = !this.selectedProjects() || (this.selectedProjects()?.length !== 1);
    }

    NavigateRequirement() {
        this.router.navigate(["app/requirement"], {
            queryParams: { project: this.selectedProjects()?.[0].id }
        });
    }    
}
