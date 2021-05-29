import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './components/admin/admin.component';
import { ClientComponent } from './components/client/client.component';
import { HomeComponent } from './components/home/home.component';
import { RepresentativeComponent } from './components/representative/representative.component';
import { UserComponent } from './components/user/user.component';

import { AdminGuard } from './guards/admin.guard';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ClientGuard } from './guards/client.guard';
import { RepresentativeGuard } from './guards/representative.guard';
import { UserGuard } from './guards/user.guard';


const routes: Routes = [
  {path: '', component: UserComponent, canActivate: [AuthenticationGuard]},
  {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]},
  {path: 'user', component: HomeComponent, canActivate: [], children: [
    {path: 'representative', component: RepresentativeComponent, canActivate: [RepresentativeGuard]},
    {path: 'client', component: ClientComponent, canActivate: [ClientGuard]},
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
