import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginComponent } from './pages/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { ThemeService } from './services/theme/theme.service';
import { Observable } from 'rxjs';
import { StyleManagerService } from './services/style-manager/style-manager.service';

function initializeAppFactory(themeService: ThemeService): void {
  themeService.setTheme('deeppurple-amber')
 }

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  providers: [
    ThemeService,
    StyleManagerService,
    // {
    //   provide: APP_INITIALIZER,
    //   deps: [ThemeService],
    //   useFactory: () => initializeAppFactory,
    //   multi: true
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
