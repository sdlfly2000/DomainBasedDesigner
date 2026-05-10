import { Component } from '@angular/core';
import { EnumInfoSeverity, StatusMessageService } from '../../../services/statusmessage.service';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { Toast } from 'primeng/toast';

@Component({
  selector: 'app-status-bar',
  templateUrl: './status-bar.component.html',
  styleUrls: ['./status-bar.component.css'],
  imports: [Toast, ButtonModule ],
  providers: [MessageService]
})
export class StatusBarComponent {

  public loginMessage: string = "";
  
  constructor(private statusMessageService: StatusMessageService, private messageService: MessageService) {
    this.statusMessageService.StatusMessage.subscribe(
      (model) =>
        this.messageService.add(
          {
            severity: model.InfoSeverity,
            summary: model.InfoSeverity,
            detail: model.Message,
            sticky: model.InfoSeverity == EnumInfoSeverity.Error
          }));
  }
}
