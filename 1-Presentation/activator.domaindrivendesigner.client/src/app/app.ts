import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StatusBarComponent } from './pages/status-bar/status-bar.component';

@Component({
  selector: 'app-root',
  imports: [
      RouterOutlet,
      StatusBarComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('WebUI');
}
