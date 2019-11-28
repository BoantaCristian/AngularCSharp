import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HighwayTollsService } from '../services/highway-tolls.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private service: HighwayTollsService) {

  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean 
    {
      if(localStorage.getItem('token') != null){
        this.service.getUser().subscribe(
          (res:any) => {
            localStorage.setItem('role', res.role[0])
          },
          err => {
            console.log(err)
          }
        )
        if(localStorage.getItem('role') == 'Admin') 
          return true
        else{
          this.router.navigateByUrl('')
          return false
        }
      }
      else {
        this.router.navigateByUrl('/user/login')
        return false
      }
  }
  
}
