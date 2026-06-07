import { Component, input } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'requirement-cmd-new',
  templateUrl: './requirement-cmd-new.component.html',
  styleUrls: ['./requirement-cmd-new.component.css'],
  imports: [ButtonModule]
})
export class RequirementCommandNewComponent {
    ProjectId = input<string>();
    ProjectName = input<string>();

    disabledCommand = false;

    constructor(private router: Router) {
    }

    NavigatenNewRequirement() {
        this.router.navigate(["app/requirement-detail"], {
            queryParams: { project: this.ProjectId(), projectName: this.ProjectName() }
        });
    }
}
