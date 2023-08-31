import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';

import { StoreModule } from '@ngrx/store';
import { navigationReducer } from './app/store/sidenav/reducers';
import { loggingInReducer } from './app/store/auth/reducers';
import { ThemeService } from './app/services/theme/theme.service';
import { StyleManagerService } from './app/services/style-manager/style-manager.service';
import { NavigationFacade } from './app/store/sidenav/facade';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginService } from './app/services/login/login.service';
import { HttpClientModule } from '@angular/common/http';


bootstrapApplication(AppComponent,
  {
    providers: [
      importProvidersFrom(StoreModule.forRoot({ navigation: navigationReducer, auth: loggingInReducer })),
      importProvidersFrom(BrowserAnimationsModule),
      importProvidersFrom(HttpClientModule),
      ThemeService,
      StyleManagerService,
      NavigationFacade,
      LoginService,
    ]
  }
  )
  .catch(err => console.error(err));