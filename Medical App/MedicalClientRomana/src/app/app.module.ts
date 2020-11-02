import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { MedicalService } from "./services/medical.service";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { HomeComponent } from './components/home/home.component';

import { MatTooltipModule, MatNativeDateModule, MatDatepickerModule, MatInputModule, MatButtonModule, MatSelectModule, MatCardModule, MatToolbarModule } from "@angular/material";
import { ToastrModule } from 'ngx-toastr';
import { AdminComponent } from './components/admin/admin.component'

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
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    ToastrModule.forRoot(),
    MatTooltipModule, MatNativeDateModule, MatDatepickerModule, MatButtonModule, MatSelectModule, MatCardModule, MatToolbarModule, MatInputModule
  ],
  providers: [MedicalService],
  bootstrap: [AppComponent]
})
export class AppModule { }
