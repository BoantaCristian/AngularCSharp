import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { HighwayTollsService } from "./services/highway-tolls.service";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { HomeComponent } from './components/home/home.component';

import { MatToolbarModule, MatCardModule, MatInputModule, MatSelectModule, MatButtonModule } from "@angular/material";
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AdminPanelComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    FormsModule, ReactiveFormsModule,
    MatToolbarModule, MatCardModule, MatInputModule, MatSelectModule, MatButtonModule
  ],
  providers: [HighwayTollsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
