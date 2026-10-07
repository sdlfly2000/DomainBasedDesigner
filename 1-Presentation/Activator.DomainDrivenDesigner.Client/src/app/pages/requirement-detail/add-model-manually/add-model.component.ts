import { Component, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'add-model-manually',
  templateUrl: './add-model.component.html',
  styleUrls: ['./add-model.component.css'],
  imports: [
      DialogModule, ButtonModule, InputTextModule, FormsModule
  ]
})
export class AddModelManuallyComponent {
    ModelNameOutput = output<string>();
    visible: boolean = false;

    ModelName: string = "";

    OnOk(): void {
        this.ModelNameOutput.emit(this.ModelName);
        this.visible = false;
    }
}

