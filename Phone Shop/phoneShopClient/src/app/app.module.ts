import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { PhoneShopService } from "./services/phone-shop.service";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { DetailDialogComponent } from './components/detail-dialog/detail-dialog.component';

import { MatAutocompleteModule, MatNativeDateModule, MatDatepickerModule, MatProgressSpinnerModule, MatBadgeModule, MatDialogModule, MatTooltipModule, MatGridListModule, MatCardModule, MatInputModule, MatSelectModule, MatToolbarModule, MatButtonModule } from "@angular/material";
import { ToastrModule } from 'ngx-toastr';
import { BagComponent } from './components/bag/bag.component';
import { FinishDailogOrderComponent } from './components/finish-dailog-order/finish-dailog-order.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    DetailDialogComponent,
    BagComponent,
    FinishDailogOrderComponent,
    AdminDashboardComponent
  ],
  entryComponents: [DetailDialogComponent, FinishDailogOrderComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({positionClass: 'toast-top-right'}),
    FormsModule, ReactiveFormsModule,
    HttpClientModule,
    MatAutocompleteModule, MatNativeDateModule, MatDatepickerModule, MatProgressSpinnerModule, MatBadgeModule, MatDialogModule, MatTooltipModule, MatGridListModule, MatCardModule, MatInputModule, MatSelectModule, MatToolbarModule, MatButtonModule
  ],
  providers: [PhoneShopService],
  bootstrap: [AppComponent]
})
export class AppModule { }
