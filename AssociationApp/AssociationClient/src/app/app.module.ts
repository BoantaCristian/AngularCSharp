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

import { MatToolbarModule, MatDialogModule, MatInputModule, MatCardModule, MatButtonModule, MatSelectModule, MatStepperModule, MatIconModule, MatPaginatorModule, MatTableModule, MatSortModule, MatMenuModule, MatTooltipModule, MatDatepickerModule, MatNativeDateModule, MatCheckboxModule } from "@angular/material";
import { AddProviderComponent } from './components/dialogs/add-provider/add-provider.component';
import { ViewDetailsComponent } from './components/dialogs/view-details/view-details.component';
import { EmitPaymentComponent } from './components/dialogs/emit-payment/emit-payment.component';
import { DisplayPaperComponent } from './components/dialogs/display-paper/display-paper.component';
import { PayComponent } from './components/dialogs/pay/pay.component';

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
    AddProviderComponent,
    ViewDetailsComponent,
    EmitPaymentComponent,
    DisplayPaperComponent,
    PayComponent
  ],
  entryComponents: [RegisterDialogComponent, LoginDialogComponent, AddAssociationComponent, AddProviderComponent, ViewDetailsComponent, EmitPaymentComponent, DisplayPaperComponent, PayComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    MatButtonModule, MatToolbarModule, MatDialogModule, MatInputModule, MatCardModule, MatSelectModule, MatStepperModule, MatIconModule, MatPaginatorModule, MatTableModule, MatSortModule, MatMenuModule, MatTooltipModule, MatDatepickerModule, MatNativeDateModule, MatCheckboxModule
  ],
  exports: [MatPaginatorModule, MatTableModule, MatSortModule],
  providers: [AssociationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
