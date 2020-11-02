import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MedicalOfficeService } from "./services/medical-office.service";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HomeComponent } from './components/home/home.component';
import { UserComponent } from './components/user/user.component';
import { RegisterComponent } from './components/User/register/register.component';
import { LoginComponent } from './components/User/login/login.component';
import { MatNativeDateModule, MatDatepickerModule, MatStepperModule, MatToolbarModule, MatButtonModule, MatSelectModule, MatCardModule, MatInputModule } from "@angular/material";
import { HttpClientModule } from "@angular/common/http";
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    RegisterComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      toastClass: 'toast toast-bootstrap-compatibility-fix'
    }),
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    MatNativeDateModule, MatDatepickerModule, MatStepperModule, MatToolbarModule, MatCardModule, MatInputModule, MatButtonModule, MatSelectModule
  ],
  providers: [MedicalOfficeService],
  bootstrap: [AppComponent]
})
export class AppModule { }
