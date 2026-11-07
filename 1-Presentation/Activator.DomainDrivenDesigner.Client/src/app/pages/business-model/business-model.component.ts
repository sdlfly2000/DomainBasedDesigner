import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { ContextService } from '../../../services/shared.Context.service';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { BusinessModelService } from './business-model.service';
import { BusinessModelModel } from './model/business-model-models';

@Component({
  selector: 'app-business-model',
    templateUrl: './business-model.component.html',
    styleUrls: ['./business-model.component.css'],
    imports: [CommonModule, FormsModule, TableModule, InputIconModule, IconFieldModule, InputTextModule, ButtonModule, DividerModule, ToolbarModule]
})
export class BusinessModelComponent implements OnInit{
    title = 'Business Model';
    IsLoading: boolean = true;
    ProjectId: string = "";
    ProjectName: string = "";

    BusinessModels: WritableSignal<BusinessModelModel[]> = signal<BusinessModelModel[]>([]);
    BusinessModel: any;

    constructor(
        private router: Router,
        private businessModelService: BusinessModelService,
        private statusMessageService: StatusMessageService,
        private contextService: ContextService) {
    }

    ngOnInit(): void {
        this.ProjectId = this.contextService.ProjectId ?? "";
        this.ProjectName = this.contextService.ProjectName ?? "";

        this.businessModelService.GetAllBusinessModelsByProject(this.ProjectId).subscribe({
            next: (response) => {
                this.BusinessModels.set(response);
                this.IsLoading = false;
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            }
        });
    }

    NavigateBusinessModelDetail(businessModel: BusinessModelModel) {
        this.router.navigate(["app/business-model-detail"], {
            queryParams: {
                project: this.ProjectId,
                projectName: this.ProjectName,
                businessModelId: businessModel.id
            }
        });
    }
}
