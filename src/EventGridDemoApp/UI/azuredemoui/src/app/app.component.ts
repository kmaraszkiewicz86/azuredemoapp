import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SendJsonComponent } from './features/send-json/component/send-json.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    SendJsonComponent
  ],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'azuredemoui';
}
