import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom, provideZoneChangeDetection } from '@angular/core';

import { ThemeService } from './app/services/theme/theme.service';
import { StyleManagerService } from './app/services/style-manager/style-manager.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginService } from './app/services/login/login.service';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app/app.routing';
import { provideRouter } from '@angular/router';

bootstrapApplication(AppComponent, {
  providers: [
    provideZoneChangeDetection(),importProvidersFrom(BrowserAnimationsModule),
    provideHttpClient(),
    ThemeService,
    StyleManagerService,
    LoginService,
    provideRouter(routes),
  ],
}).catch((err) => console.error(err));
