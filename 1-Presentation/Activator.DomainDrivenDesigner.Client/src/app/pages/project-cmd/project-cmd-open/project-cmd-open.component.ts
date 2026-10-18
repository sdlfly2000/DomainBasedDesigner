import { Component, input, OnChanges, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ContextService } from '../../../../services/shared.Context.service';
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
        private router: Router,
        private contextService: ContextService) {
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.disabledCommand = !this.selectedProjects() || (this.selectedProjects()?.length !== 1);
    }

    NavigateRequirement() {

        this.contextService.ProjectId = this.selectedProjects()?.[0].id;
        this.contextService.ProjectName = this.selectedProjects()?.[0].name;

        this.router.navigate(["app/requirement"], {
            queryParams: {
                project: this.selectedProjects()?.[0].id,
                projectName: this.selectedProjects()?.[0].name
            }
        });
    }    
}
