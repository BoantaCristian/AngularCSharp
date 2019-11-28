import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../services/user.service";
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  values
  formModel={
    UserName: '',
    Password: ''
  }
  constructor(private toastr: ToastrService, private service: UserService, private router: Router) { }

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
      },
      err =>{
        if(err.status == 400)
          this.toastr.error('Incorrect username or password','Authentication failed')
      }
    )
  }

}
