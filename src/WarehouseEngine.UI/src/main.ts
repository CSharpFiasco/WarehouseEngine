import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom, isDevMode } from '@angular/core';

import { StoreModule } from '@ngrx/store';
import { navigationReducer } from './app/store/layout/sidenav/sidenav.reducers';
import { loggingInReducer } from './app/store/auth/reducers';
import { ThemeService } from './app/services/theme/theme.service';
import { StyleManagerService } from './app/services/style-manager/style-manager.service';
import { LayoutFacade } from './app/store/layout/facade';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginService } from './app/services/login/login.service';
import { HttpClientModule } from '@angular/common/http';
import { provideStoreDevtools } from '@ngrx/store-devtools';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(
      StoreModule.forRoot({
        navigation: navigationReducer,
        auth: loggingInReducer,
      })
    ),
    importProvidersFrom(BrowserAnimationsModule),
    importProvidersFrom(HttpClientModule),
    ThemeService,
    StyleManagerService,
    LayoutFacade,
    LoginService,
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
  ],
}).catch((err) => console.error(err));
