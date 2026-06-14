import { Component, input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RequirementDetailCommandService } from '../requirement-detail-cmd.service';
import { AnalyzeRequirementsRequestsModel } from '../model/requirement-detail-cmd';

@Component({
  standalone: true,
  selector: 'requirement-detail-cmd-analyze',
  templateUrl: './requirement-detail-cmd-analyze.component.html',
  styleUrls: ['./requirement-detail-cmd-analyze.component.css'],
  imports: [ButtonModule]
})
export class RequirementDetailCommandAnalyzeComponent {
    Description = input<string>();

    disabledCommand = false;

    constructor(private service: RequirementDetailCommandService) {
    }

    Analyze(): void {
        let request: AnalyzeRequirementsRequestsModel = { description: this.Description() ?? "" };
        this.service.AnalyzeRequirement(request).subscribe({});
    }
}
