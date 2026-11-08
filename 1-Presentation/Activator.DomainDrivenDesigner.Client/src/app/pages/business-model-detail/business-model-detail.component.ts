import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import mermaid from 'mermaid';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { SelectModule } from 'primeng/select';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { ToolbarModule } from 'primeng/toolbar';
import { ContextService } from '../../../services/shared.Context.service';
import { QueryStringService } from '../../../services/shared.QueryString.service';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { BusinessModelModel, CreateDefaultBusinessModel } from '../business-model/model/business-model-models';
import { BusinessModelDetailService } from './business-model-detail.service';

@Component({
    selector: 'app-business-model-detail',
    templateUrl: './business-model-detail.component.html',
    styleUrls: ['./business-model-detail.component.css'],
    imports: [
      FormsModule, ButtonModule, DividerModule, TextareaModule, ToolbarModule, TabsModule, SelectModule, FloatLabelModule 
  ]
})
export class BusinessModelDetailComponent implements AfterViewInit {
    title = 'Business Model Detail';
    ProjectId : string | undefined;
    ProjectName: string | undefined;
    BusinessModel: WritableSignal<BusinessModelModel> = signal<BusinessModelModel>(CreateDefaultBusinessModel())

    private graphDefinition: string = '';
    
    @ViewChild('mermaidContainer', { static: false }) mermaidContainer!: ElementRef<HTMLDivElement>;

    constructor(
        private queryStringService: QueryStringService,
        private contextService: ContextService,
        private statusMessageService: StatusMessageService,
        private businessModelDetailService: BusinessModelDetailService,
        private cdr: ChangeDetectorRef) {
    }

    ngAfterViewInit(): void {
        this.businessModelDetailService.GetBusinessModelsById(this.BusinessModel().id).subscribe({
            next: async (businessModel) => {
                this.BusinessModel.set(businessModel);
                this.graphDefinition = this.applyMermaidClassDefinition(this.BusinessModel().contentMermaid);
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

    ngOnInit(): void {
        this.ProjectId = this.contextService.ProjectId;
        this.ProjectName = this.contextService.ProjectName;

        this.BusinessModel().id = this.queryStringService.Get("businessModelId") ?? '';

        mermaid.initialize({
            startOnLoad: false,          // Stops automatic selector scanning
            suppressErrorRendering: true // Stops DOM injection of "dmermaid-XXXX" elements
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
