import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom, isDevMode } from '@angular/core';

import { StoreModule } from '@ngrx/store';
import { ThemeService } from './app/services/theme/theme.service';
import { StyleManagerService } from './app/services/style-manager/style-manager.service';
import { LayoutFacade } from './app/store/layout/facade';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginService } from './app/services/login/login.service';
import { HttpClientModule } from '@angular/common/http';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { routes } from './app/app.routing';
import { provideRouter } from '@angular/router';
import { reducers, type WarehouseEngineStore } from './app/store/initial-state';
import { EffectsModule } from '@ngrx/effects';
import { AuthEffects } from './app/store/auth/auth.effects';
import { AuthFacade } from './app/store/auth/auth.facade';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(StoreModule.forRoot<WarehouseEngineStore>(reducers), EffectsModule.forRoot([AuthEffects])),
    importProvidersFrom(BrowserAnimationsModule),
    importProvidersFrom(HttpClientModule),
    ThemeService,
    StyleManagerService,
    LayoutFacade,
    AuthFacade,
    LoginService,
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideRouter(routes),
  ],
}).catch((err) => console.error(err));
