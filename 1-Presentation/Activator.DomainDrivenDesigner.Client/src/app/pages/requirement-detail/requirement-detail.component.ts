import { afterNextRender, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StatusMessageService } from '../../../services/statusmessage.service';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { TextareaModule } from 'primeng/textarea';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';
import mermaid from 'mermaid';
import { RequirementDetailService } from './requirement-detail.service';
import { RequirementDetailCommandAnalyzeComponent } from '../requirement-detail-cmd/requirement-detail-cmd-analyze/requirement-detail-cmd-analyze.component';
import { ToolbarModule } from 'primeng/toolbar';

@Component({
  selector: 'app-requirement-detail',
  templateUrl: './requirement-detail.component.html',
  styleUrls: ['./requirement-detail.component.css'],
    imports: [FormsModule, DividerModule, TextareaModule, ToolbarModule, RequirementDetailCommandAnalyzeComponent]
})
export class RequirementDetailComponent implements OnInit{
    title = 'Requirement Detail';
    ProjectId : string = '';
    ProjectName : string = '';
    RequirementDescription: string = '';
    AnalyzedResult: string = "";

    private graphDefinition = `
    graph TD
      A[Start Project] --> B{Is it Angular?}
      B -- Yes --> C[Use afterNextRender]
      B -- No --> D[Check other frameworks]
  `;

    @ViewChild('mermaidContainer', { static: false }) mermaidContainer!: ElementRef<HTMLDivElement>;

    constructor(
        private requirementDetailService: RequirementDetailService,
        private queryStringService: QueryStringService,
        private statusMessageService: StatusMessageService) {

        afterNextRender(async () => {
            mermaid.initialize({
                startOnLoad: false,
                theme: 'default',
                securityLevel: 'loose'
            });

            await this.renderDiagram();
        });
    }

    ngOnInit(): void {
        this.ProjectId = this.queryStringService.Get('projectId') ?? "";
        this.ProjectName = this.queryStringService.Get("projectName") ?? "";

    }

    async OnAnalyzedResult(analyzedResult: string) {
        this.AnalyzedResult = analyzedResult;
        this.graphDefinition = `
        graph TD
          A[Start Project] --> B{Is it Angular?}
          B -- Yes --> C[Use afterNextRender]
          B -- No --> D[Check other frameworks / standard]
      `
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
