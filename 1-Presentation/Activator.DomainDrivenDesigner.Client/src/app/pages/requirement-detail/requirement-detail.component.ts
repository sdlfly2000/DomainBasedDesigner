import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import mermaid from 'mermaid';
import { DividerModule } from 'primeng/divider';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { ToolbarModule } from 'primeng/toolbar';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { StatusMessageService } from '../../../services/statusmessage.service';
import { AnalyzeRequirementsResponseModel } from '../requirement-detail-cmd/model/requirement-detail-cmd';
import { RequirementDetailCommandAnalyzeComponent } from '../requirement-detail-cmd/requirement-detail-cmd-analyze/requirement-detail-cmd-analyze.component';
import { RequirementDetailService } from './requirement-detail.service';

@Component({
  selector: 'app-requirement-detail',
  templateUrl: './requirement-detail.component.html',
  styleUrls: ['./requirement-detail.component.css'],
  imports: [FormsModule, DividerModule, TextareaModule, ToolbarModule, TabsModule, RequirementDetailCommandAnalyzeComponent]
})
export class RequirementDetailComponent implements OnInit{
    title = 'Requirement Detail';
    ProjectId : string = '';
    ProjectName : string = '';
    RequirementDescription: string = '';
    AnalyzedResult: AnalyzeRequirementsResponseModel = { businessModels: [], raw: '' };
    ModelMermaidRaws: string[] = [];

    private graphDefinition: string = '';
    
    @ViewChild('mermaidContainer', { static: false }) mermaidContainer!: ElementRef<HTMLDivElement>;

    constructor(
        private requirementDetailService: RequirementDetailService,
        private queryStringService: QueryStringService,
        private statusMessageService: StatusMessageService) {

    }

    ngOnInit(): void {
        this.ProjectId = this.queryStringService.Get('projectId') ?? "";
        this.ProjectName = this.queryStringService.Get("projectName") ?? "";

        mermaid.initialize({
            startOnLoad: false,          // Stops automatic selector scanning
            suppressErrorRendering: true // Stops DOM injection of "dmermaid-XXXX" elements
        });

    }

    async OnAnalyzedResult(analyzedResult: AnalyzeRequirementsResponseModel) {
        this.AnalyzedResult = analyzedResult;
        this.graphDefinition = analyzedResult.raw;
        await this.renderDiagram();
    }

    async OnAnalyzedResultChange(changedValue: string) {
        this.graphDefinition = changedValue;
        await this.renderDiagram();
    }

    async OnModelTabClick(model: string | number | undefined) {
        let index: number = this.AnalyzedResult.businessModels.findIndex(m => m.name === model);
        this.graphDefinition = this.ModelMermaidRaws[index];
        if (this.graphDefinition == undefined || this.graphDefinition == '') {
            return;
        }
        await this.renderDiagram();
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
}
