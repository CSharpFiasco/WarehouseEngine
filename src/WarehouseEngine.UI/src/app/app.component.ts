import { Component } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginStatus } from './types/login-status.type';
import { ThemeService } from './services/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  protected loggedIn$: BehaviorSubject<LoginStatus | undefined> = new BehaviorSubject<LoginStatus | undefined>(undefined);
  constructor(themeService: ThemeService){
    themeService.setTheme('indigo-pink');
  }
}
