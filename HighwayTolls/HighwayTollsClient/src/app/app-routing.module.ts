import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { UserComponent } from './components/user/user.component';
import { RegisterComponent } from './components/user/register/register.component';
import { LoginComponent } from './components/user/login/login.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { AuthGuard } from './auth/auth.guard';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'user', component: UserComponent,
   children: [
     {path: 'register', component:RegisterComponent},
     {path:'login', component:LoginComponent}
   ]
  },
  {path: 'admin', component: AdminPanelComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
