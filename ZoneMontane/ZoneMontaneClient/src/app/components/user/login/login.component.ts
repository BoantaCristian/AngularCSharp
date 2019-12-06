import { Component, OnInit } from '@angular/core';
import { MountainZoneService } from 'src/app/services/mountain-zone.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formModel = {
    UserName: '',
    Password: ''
  }

  constructor(private service: MountainZoneService, private router: Router) { }

  ngOnInit() {
    if(localStorage.getItem('token') != null){
      this.router.navigateByUrl('')
    }
  }

  login(){
    this.service.login(this.formModel).subscribe(
      (res:any) => {
        localStorage.setItem('token', res.token)
        this.router.navigateByUrl('')
      }
    )
  }

}
