import { Component, OnInit } from '@angular/core';
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private toastr: ToastrService, private service: PhoneShopService, private router: Router) { }

  formModel = {
    UserName: '',
    Password: ''
  }

  ngOnInit() {
    this.userLogged()
  }
  userLogged(){
    if(localStorage.getItem('token'))
      this.router.navigateByUrl('')
  }
  login(){
    this.service.login(this.formModel).subscribe(
      (res: any) =>{
          localStorage.setItem('token', res.token)
          this.formModel.UserName = ''
          this.formModel.Password = ''
          this.router.navigateByUrl('')
      },
      err => {
        if(err.status == 400){
          this.toastr.error('Incorrect username or password', 'Authentication failed!')
        }
      }
    )
  }

}
