import { Component, input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../../services/statusmessage.service';
import { SaveRequirementRequestModel } from '../model/requirement-detail-cmd';
import { RequirementDetailCommandService } from '../requirement-detail-cmd.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'requirement-detail-cmd-save',
  templateUrl: './requirement-detail-cmd-save.component.html',
  styleUrls: ['./requirement-detail-cmd-save.component.css'],
  imports: [ButtonModule]
})
export class RequirementDetailCommandSaveComponent {
    Description = input<string>();

    ProjectId = input<string>();
    RequirementId = input<string>();

    constructor(
        private service: RequirementDetailCommandService,
        private statusMessageService: StatusMessageService) {
    }

    Save(): void {
        let request: SaveRequirementRequestModel = {
            projectId: this.ProjectId(),
            requirementId: this.RequirementId(),
            description: this.Description()
        }

        this.statusMessageService.IsLoading = true;

        this.service.SaveRequirement(request).subscribe({
            next: (response) => {
                this.statusMessageService.StatusMessage = new StatusMessageModel("Requirement saved successfully.", EnumInfoSeverity.Success);
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
                this.statusMessageService.IsLoading = false;
            },
            complete: () => {
                this.statusMessageService.IsLoading = false;
            }
        });
    }
}
