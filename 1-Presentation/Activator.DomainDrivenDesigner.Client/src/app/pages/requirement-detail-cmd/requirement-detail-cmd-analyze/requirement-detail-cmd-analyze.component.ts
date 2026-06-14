import { Component, input, output, signal, WritableSignal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RequirementDetailCommandService } from '../requirement-detail-cmd.service';
import { AnalyzeRequirementsRequestModel } from '../model/requirement-detail-cmd';
import { HttpErrorResponse } from '@angular/common/http';
import { StatusMessageModel, EnumInfoSeverity, StatusMessageService } from '../../../../services/statusmessage.service';

@Component({
  standalone: true,
  selector: 'requirement-detail-cmd-analyze',
  templateUrl: './requirement-detail-cmd-analyze.component.html',
  styleUrls: ['./requirement-detail-cmd-analyze.component.css'],
  imports: [ButtonModule]
})
export class RequirementDetailCommandAnalyzeComponent {
    Description = input<string>();

    AnalyzedResult = output<string>();

    disabledCommand = false;

    constructor(
        private service: RequirementDetailCommandService,
        private statusMessageService: StatusMessageService) {
    }

    Analyze(): void {
        let request: AnalyzeRequirementsRequestModel = { description: this.Description() ?? "" };
        this.service.AnalyzeRequirement(request).subscribe({
            next: (response) => {
                this.AnalyzedResult.emit(response.raw);
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            }
        });
    }
}
