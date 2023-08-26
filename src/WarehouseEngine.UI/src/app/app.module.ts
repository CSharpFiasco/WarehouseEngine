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
import { StyleManagerService } from './services/style-manager/style-manager.service';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NavigationFacade } from './store/sidenav/facade';
import { navigationReducer } from './store/sidenav/reducers';
import { StoreModule } from '@ngrx/store';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { AppRoutingModule } from './app-routing.module';
import { loggingInReducer } from './store/auth/reducers';
import { HttpClientModule } from '@angular/common/http';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    TopNavComponent,
    SidenavComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    AppRoutingModule,
    StoreModule.forRoot({ navigation: navigationReducer, auth: loggingInReducer }),
    HttpClientModule,
    MatMenuModule
  ],
  providers: [
    ThemeService,
    StyleManagerService,
    NavigationFacade
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
