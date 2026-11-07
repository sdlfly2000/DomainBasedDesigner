import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import mermaid from 'mermaid';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { SelectModule } from 'primeng/select';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { ToolbarModule } from 'primeng/toolbar';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { AnalyzeRequirementsResponseModel, BusinessModel, BusinessModelInstance } from '../requirement-detail-cmd/model/requirement-detail-cmd';
import { RequirementDetailCommandAnalyzeComponent } from '../requirement-detail-cmd/requirement-detail-cmd-analyze/requirement-detail-cmd-analyze.component';
import { RequirementDetailCommandSaveComponent } from '../requirement-detail-cmd/requirement-detail-cmd-save/requirement-detail-cmd-save.component';

@Component({
  selector: 'app-requirement-detail',
  templateUrl: './requirement-detail.component.html',
  styleUrls: ['./requirement-detail.component.css'],
  imports: [
      FormsModule, ButtonModule, DividerModule, TextareaModule, ToolbarModule, TabsModule, SelectModule, FloatLabelModule,
      RequirementDetailCommandAnalyzeComponent, RequirementDetailCommandSaveComponent
  ]
})
export class BusinessModelDetailComponent implements AfterViewInit {
    title = 'Business Model Detail';
    ProjectId : string = '';
    ProjectName: string = '';
    RequirementId: string = '';
    RequirementDescription: string = '';
    AnalyzedResult: AnalyzeRequirementsResponseModel = { businessModels: [], raw: '' };
    ModelIdList: (string | undefined)[] = [];
    ModelMermaidRawsTab: string[] = [];
    ContextIdTab: (string | undefined)[] = [];
    CurrentContextName: string = ''
    CurrentModelName: string = '';
    CurrentTabIndex: number = 0;
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
        private queryStringService: QueryStringService,
        private statusMessageService: StatusMessageService,
        private cdr: ChangeDetectorRef) {
    }
    ngAfterViewInit(): void {

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
        this.ModelMermaidRawsTab = [];
        this.AnalyzedResult = analyzedResult;
        this.graphDefinition = this.applyMermaidClassDefinition(analyzedResult.raw);
        await this.renderDiagram();
    }

    async OnAnalyzedResultChange(changedValue: string) {
        this.graphDefinition = this.applyMermaidClassDefinition(changedValue);
        await this.renderDiagram();
    }
       

    OnCreateContext() {
        let tabIndex: number = this.AnalyzedResult.businessModels.findIndex(m => m.name === this.CurrentModelName);
        this.CurrentContextName = this.ContextIdTab[tabIndex] ?? "";
    }

    SaveModel() {
        let tabIndex: number = this.AnalyzedResult.businessModels.findIndex(m => m.name === this.CurrentModelName);
        this.CurrentBusinessModel.id = this.ModelIdList[tabIndex];
        this.CurrentBusinessModel.contentMermaid = this.ModelMermaidRawsTab[tabIndex]
        this.CurrentBusinessModel.name = this.CurrentModelName;
        this.CurrentBusinessModel.contextId = this.ContextIdTab[tabIndex];

        if (!BusinessModelInstance.NotNullValidate(this.CurrentBusinessModel)) {
            this.statusMessageService.StatusMessage = new StatusMessageModel("Invalid business model data - Please fill in all required fields", EnumInfoSeverity.Error);
            return;
        }
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
