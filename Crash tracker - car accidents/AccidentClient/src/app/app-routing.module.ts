import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './components/admin/admin.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { UserComponent } from './components/user/user.component';
import { AdminGuard } from './guard/admin.guard';
import { GuardGuard } from './guard/guard.guard';
import { LoginGuard } from './guard/login.guard';


const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [GuardGuard]},
  {path: 'user', component: UserComponent, children: [{path: 'login', component: LoginComponent,  canActivate: [LoginGuard]}, {path: 'register', component: RegisterComponent}]},
  {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]}, 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
