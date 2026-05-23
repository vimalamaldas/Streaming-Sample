import { Component } from '@angular/core';
import { StreamingComponent } from './streaming.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  imports: [StreamingComponent]
})
export class AppComponent {}
