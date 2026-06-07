import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { RequirementService } from './requirement.service';
import { RequirementModel } from './model/requirement';
import { QueryStringService } from '../../../services/shared.QueryString.service';

@Component({
  selector: 'app-requirement',
  templateUrl: './requirement.component.html',
  styleUrls: ['./requirement.component.css'],
    imports: [CommonModule, FormsModule, TableModule, InputIconModule, IconFieldModule, InputTextModule, ButtonModule, DividerModule, ToolbarModule]
})
export class RequirementComponent implements OnInit{
    title = 'Requirements';
    IsLoading: boolean = true;

    Requirements: WritableSignal<RequirementModel[]> = signal<RequirementModel[]>([]);

    constructor(
        private queryStringService: QueryStringService,
        private requirementService: RequirementService,
        private statusMessageService: StatusMessageService) {
    }

    ngOnInit(): void {
        let projectId = this.queryStringService.Get("project") ?? "";

        this.requirementService.GetAllRequirements(projectId).subscribe({
            next: (response) => {
                this.Requirements.set(response.requirements);
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
