import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import mermaid from 'mermaid';
import { DividerModule } from 'primeng/divider';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { ToolbarModule } from 'primeng/toolbar';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { AnalyzeRequirementsResponseModel, BusinessModel } from '../requirement-detail-cmd/model/requirement-detail-cmd';
import { RequirementDetailCommandAnalyzeComponent } from '../requirement-detail-cmd/requirement-detail-cmd-analyze/requirement-detail-cmd-analyze.component';
import { RequirementDetailCommandSaveComponent } from '../requirement-detail-cmd/requirement-detail-cmd-save/requirement-detail-cmd-save.component';
import { RequirementDetailService } from './requirement-detail.service';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { Context, CreateContextRequest } from './model/requirement-detail';

@Component({
  selector: 'app-requirement-detail',
  templateUrl: './requirement-detail.component.html',
  styleUrls: ['./requirement-detail.component.css'],
  imports: [
      FormsModule, ButtonModule, DividerModule, TextareaModule, ToolbarModule, TabsModule, SelectModule,
      RequirementDetailCommandAnalyzeComponent, RequirementDetailCommandSaveComponent
  ]
})
export class RequirementDetailComponent implements AfterViewInit {
    title = 'Requirement Detail';
    ProjectId : string = '';
    ProjectName: string = '';
    RequirementId: string = '';
    RequirementDescription: string = '';
    AnalyzedResult: AnalyzeRequirementsResponseModel = { businessModels: [], raw: '' };
    ModelIdList: (string | undefined)[] = [];
    ModelMermaidRaws: string[] = [];
    Contexts: Context[] = []
    ContextNames : string[] = []
    CurrentContext: Context
    CurrentContextName: string = ''
    CurrentModelName: string = '';
    CurrentBusinessModel: BusinessModel = {
        id: '',
        name: '',
        contentMermaid: '',
        contextId: '',
        contextName: '',
        createdOnUtc: undefined
    };

    private graphDefinition: string = '';
    
    @ViewChild('mermaidContainer', { static: false }) mermaidContainer!: ElementRef<HTMLDivElement>;

    constructor(
        private requirementDetailService: RequirementDetailService,
        private queryStringService: QueryStringService,
        private statusMessageService: StatusMessageService,
        private cdr: ChangeDetectorRef) {

        this.CurrentContext = {
            id: '',
            name: '',
            projectId: ''
        }
    }
    ngAfterViewInit(): void {
        if (this.RequirementId != "") {
            this.requirementDetailService.RetrieveRequirement(this.RequirementId).subscribe({
                next: (response) => {
                    this.RequirementDescription = response.description;
                },
                error: (error) => {
                    if (error instanceof HttpErrorResponse) {
                        this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                    }
                },
                complete: () => this.cdr.detectChanges()
            });
        }

        this.requirementDetailService.RetrieveContexts(this.ProjectId).subscribe({
            next: (contexts) => {
                this.Contexts = contexts;
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            },
            complete: () => this.cdr.detectChanges()
        })
    }

    ngOnInit(): void {
        this.ProjectId = this.queryStringService.Get('project') ?? "";
        this.ProjectName = this.queryStringService.Get("projectName") ?? "";
        this.RequirementId = this.queryStringService.Get("requirementId") ?? "";

        mermaid.initialize({
            startOnLoad: false,          // Stops automatic selector scanning
            suppressErrorRendering: true // Stops DOM injection of "dmermaid-XXXX" elements
        });
    }

    async OnAnalyzedResult(analyzedResult: AnalyzeRequirementsResponseModel) {
        this.AnalyzedResult = analyzedResult;
        this.graphDefinition = this.applyMermaidClassDefinition(analyzedResult.raw);
        await this.renderDiagram();
    }

    async OnAnalyzedResultChange(changedValue: string) {
        this.graphDefinition = this.applyMermaidClassDefinition(changedValue);
        await this.renderDiagram();
    }

    async OnModelTabClick(model: string | number | undefined) {
        this.CurrentModelName = model as string;
        let index: number = this.AnalyzedResult.businessModels.findIndex(m => m.name === model);
        if (this.ModelMermaidRaws[index] == undefined || this.ModelMermaidRaws[index] == '') {
            this.requirementDetailService.RetrieveBusinessModel(this.CurrentModelName, this.RequirementId).subscribe({
                next: async (model) => {
                    if (model == undefined || model == null) {
                        this.CurrentBusinessModel = {
                            id: '',
                            name: '',
                            contentMermaid: '',
                            contextId: '',
                            contextName: '',
                            createdOnUtc: undefined
                        };
                        return;
                    }
                    this.ModelIdList[index] = model.id;
                    this.CurrentBusinessModel = model;
                    this.ModelMermaidRaws[index] = model.contentMermaid != undefined ? model.contentMermaid : '';
                    this.graphDefinition = this.applyMermaidClassDefinition(this.ModelMermaidRaws[index]);
                    await this.renderDiagram();
                },
                error: (error) => {
                    if (error instanceof HttpErrorResponse) {
                        this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                    }
                },
                complete: () => this.cdr.detectChanges()
            });
        }
    }

    OnCreateContext() {
        let request: CreateContextRequest = {
            name: this.CurrentContextName,
            projectId: this.ProjectId
        };
        this.requirementDetailService.CreateContext(request).subscribe({
            next: (contextId) => {
                this.CurrentContext = {
                    id: contextId,
                    name: this.CurrentContext.name ?? '',
                    projectId: this.ProjectId   
                }
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            },
            complete: () => this.cdr.detectChanges()
        });
    }

    SaveModel() {
        let tabIndex: number = this.AnalyzedResult.businessModels.findIndex(m => m.name === this.CurrentModelName);
        this.CurrentBusinessModel.id = this.ModelIdList[tabIndex];
        this.CurrentBusinessModel.contentMermaid = this.ModelMermaidRaws[tabIndex]
        this.CurrentBusinessModel.name = this.CurrentModelName;
        this.requirementDetailService.UpsertBusinessModel(this.CurrentBusinessModel, this.RequirementId).subscribe({
            next: (success) => {
                if (success) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel("Success to Save Description", EnumInfoSeverity.Info);
                } else {
                    this.statusMessageService.StatusMessage = new StatusMessageModel("Fail to Save Description", EnumInfoSeverity.Warn);
                }
            },
            error: (error) => {
                if (error instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(error.message, EnumInfoSeverity.Error);
                }
            }
        });
    }

    private async renderDiagram() {
        try {
            const element = this.mermaidContainer.nativeElement;
            const uniqueId = 'mermaid-' + Math.floor(Math.random() * 10000);

            // Programmatically render the text definition into SVG code
            const { svg, bindFunctions } = await mermaid.render(uniqueId, this.graphDefinition);

            element.innerHTML = svg;

            // Necessary if your graph relies on interactive elements like click callbacks
            if (bindFunctions) {
                bindFunctions(element);
            }
        } catch (error) {
            console.error('Mermaid parsing failed:', error);
        }
    }

    private applyMermaidClassDefinition(content: string): string {
        if (content == undefined || content == '') {
            return '';
        }

        if (content.startsWith("classDiagram")) {
            return content;
        }

        let mermaidContent = "classDiagram\n".concat(content);
        return mermaidContent;
    }
}
