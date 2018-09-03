import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './authenticate-xhr.backend';

import { routing } from './app.routing';


/* App Root */
import { AppComponent } from './app.component';
import { HomeComponent } from './login/home/home.component';
import { HeaderComponent } from './header/header.component';

/* Account Imports */
import { AccountModule }  from './login/account/account.module';
/* Dashboard Imports */
import { DashboardModule }  from './login/dashboard/dashboard.module';

import { ConfigService } from './services/config.service';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent      
  ],
  imports: [
    AccountModule,
    DashboardModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    routing
  ],
  providers: [ConfigService, { 
    provide: XHRBackend, 
    useClass: AuthenticateXHRBackend
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
