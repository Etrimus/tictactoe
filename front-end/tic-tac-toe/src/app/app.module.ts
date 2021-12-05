import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BASE_API_URL } from './generated/clients';
import { HomeComponent } from './home/home.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { UserBageComponent } from './user-bage/user-bage.component';

@NgModule({
  declarations: [
    AppComponent, HomeComponent, SignInComponent, UserBageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    { provide: BASE_API_URL, useValue: 'https://localhost:5001' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }