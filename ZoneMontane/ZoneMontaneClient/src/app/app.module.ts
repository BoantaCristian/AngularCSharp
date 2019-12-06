import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from "@angular/common/http";
import { MountainZoneService } from "./services/mountain-zone.service";

import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { HomeComponent } from './components/home/home.component';

import { MatButtonModule, MatCardModule, MatSelectModule, MatInputModule, MatToolbarModule } from "@angular/material";
import { ToastrModule } from 'ngx-toastr';
import { AdminComponent } from './admin/admin.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    MatButtonModule, MatCardModule, MatSelectModule, MatInputModule, MatToolbarModule
  ],
  providers: [MountainZoneService],
  bootstrap: [AppComponent]
})
export class AppModule { }
