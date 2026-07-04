import { HttpErrorResponse } from '@angular/common/http';
import { Component, input, output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../../services/statusmessage.service';
import { AnalyzeRequirementsResponseModel } from '../model/requirement-detail-cmd';
import { RequirementDetailCommandService } from '../requirement-detail-cmd.service';

@Component({
  standalone: true,
  selector: 'requirement-detail-cmd-save',
  templateUrl: './requirement-detail-cmd-save.component.html',
  styleUrls: ['./requirement-detail-cmd-save.component.css'],
  imports: [ButtonModule]
})
export class RequirementDetailCommandSaveComponent {
    Description = input<string>();

    constructor(
        private service: RequirementDetailCommandService,
        private statusMessageService: StatusMessageService) {
    }

    Save(): void {

    }
}
