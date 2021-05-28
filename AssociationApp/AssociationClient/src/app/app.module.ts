import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AssociationService } from "./services/association.service";

import { AdminComponent } from './components/admin/admin.component';
import { ClientComponent } from './components/client/client.component';
import { RepresentativeComponent } from './components/representative/representative.component';
import { UserComponent } from './components/user/user.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterDialogComponent } from './components/dialogs/register-dialog/register-dialog.component';
import { LoginDialogComponent } from './components/dialogs/login-dialog/login-dialog.component';
import { AddAssociationComponent } from './components/dialogs/add-association/add-association.component';

import { MatToolbarModule, MatDialogModule, MatInputModule, MatCardModule, MatButtonModule, MatSelectModule, MatStepperModule } from "@angular/material";
import { AddProviderComponent } from './components/dialogs/add-provider/add-provider.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    ClientComponent,
    RepresentativeComponent,
    UserComponent,
    HomeComponent,
    RegisterDialogComponent,
    LoginDialogComponent,
    AddAssociationComponent,
    AddProviderComponent
  ],
  entryComponents: [RegisterDialogComponent, LoginDialogComponent, AddAssociationComponent, AddProviderComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    MatButtonModule, MatToolbarModule, MatDialogModule, MatInputModule, MatCardModule, MatSelectModule, MatStepperModule
  ],
  providers: [AssociationService],
  bootstrap: [AppComponent]
})
export class AppModule { }