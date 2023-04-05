import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeModule } from './home/home.module';
import { LoginModule } from './login/login.module';
import { RegisterModule } from './register/register.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { BASE_PATH as TENNIS_SERVICE_BASE_PATH } from 'src/app/core/api/tennis-service';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    RegisterModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    LoginModule,
    HomeModule,
    AppRoutingModule
  ],
  providers: [
    {provide : TENNIS_SERVICE_BASE_PATH, useValue: 'https://localhost:5001'},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
