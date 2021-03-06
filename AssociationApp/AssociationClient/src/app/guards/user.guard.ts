import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserGuard implements CanActivate {
  constructor(private router: Router) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if(localStorage.getItem('token') != null){
        if(localStorage.getItem('role') == 'Client'){
          this.router.navigateByUrl('user/client')
          return true
        }
        if(localStorage.getItem('role') == 'Admin')
        {
          this.router.navigateByUrl('admin')
          return false
        }
        if(localStorage.getItem('role') != 'Admin' && localStorage.getItem('role') != 'Client')
        {
          this.router.navigateByUrl('user/representative')
          return true
        }
      }
      else
      {
        this.router.navigateByUrl('')
        return false
      } 
    }
  
}
