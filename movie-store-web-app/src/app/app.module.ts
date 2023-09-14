import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CustomerModule } from './customer/customer.module';
import { MovieModule } from './movie/movie.module';
import { NavbarComponent } from './navbar/navbar.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list'
import { MatButtonModule } from '@angular/material/button'
import { MatFormFieldModule } from '@angular/material/form-field'
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { InteractionType, PublicClientApplication } from '@azure/msal-browser';
import { MsalInterceptor, MsalModule, MsalRedirectComponent } from '@azure/msal-angular';
import { HomeComponent } from './home/home.component';
import { MatCardModule } from '@angular/material/card';
import { AzureHomeService } from './service/azure-home.service';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CustomerModule,
    MovieModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatCardModule,
    MatSelectModule,
    MsalModule.forRoot(new PublicClientApplication
      (
        {
          auth: {
            clientId: '4e1ff54b-bf34-4f45-83ce-e50fc32967cd',
            authority: 'https://login.microsoftonline.com/common',
            redirectUri: 'http://localhost:4200',
          },
          cache: {
            cacheLocation: 'localStorage'
          }
        }
      ), null!,
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: new Map(
          [
            ['https://localhost:7064', ['api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read',
              'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.write']]
          ]
        )
      })
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: MsalInterceptor,
    multi: true
  }, AzureHomeService],

  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
