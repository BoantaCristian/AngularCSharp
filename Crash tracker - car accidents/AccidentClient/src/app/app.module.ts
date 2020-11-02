import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home/home.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';

import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AccidentService } from "./services/accident.service";
import { ToastrModule } from 'ngx-toastr';

import { MatCardModule, MatTooltipModule, MatGridListModule, MatStepperModule, MatDatepickerModule, MatNativeDateModule, MatInputModule, MatSelectModule, MatButtonModule, MatDialogModule } from "@angular/material";
import { AdminComponent } from './components/admin/admin.component';
import { RegisterDialogComponent } from './components/register-dialog/register-dialog.component';
import { PeopleDialogComponent } from './components/people-dialog/people-dialog.component';
import { AccidentDetailsDialogComponent } from './components/accident-details-dialog/accident-details-dialog.component';
import { AddAccidentDialogComponent } from './components/add-accident-dialog/add-accident-dialog.component';
import { AddAgentDialogComponent } from './components/add-agent-dialog/add-agent-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,
    RegisterDialogComponent,
    PeopleDialogComponent,
    AccidentDetailsDialogComponent,
    AddAccidentDialogComponent,
    AddAgentDialogComponent
  ],
  entryComponents: [RegisterDialogComponent, PeopleDialogComponent, AccidentDetailsDialogComponent, AddAccidentDialogComponent, AddAgentDialogComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    MatCardModule, MatTooltipModule, MatStepperModule, MatGridListModule, MatDatepickerModule, MatNativeDateModule, MatInputModule, MatSelectModule, MatButtonModule, MatDialogModule,
    ToastrModule.forRoot()
  ],
  providers: [AccidentService],
  bootstrap: [AppComponent]
})
export class AppModule { }
