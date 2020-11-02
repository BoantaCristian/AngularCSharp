import { Component, OnInit } from '@angular/core';
import { MedicalOfficeService } from 'src/app/services/medical-office.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private service: MedicalOfficeService, private router: Router, private toastr: ToastrService) { }

  formModel = {
    UserName: '',
    Password: ''
  }

  ngOnInit() {
    this.checkUser()
  }
  checkUser(){
    if(localStorage.getItem('token') != null)
      this.router.navigateByUrl('')
  }
  login(){
    this.service.login(this.formModel).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token)
        this.router.navigateByUrl('')
      },
      err => {
        console.log(err)
        if(err.status == 400){
          this.toastr.error('Incorrect username or password','Authentication failed')
        }
      }
    )
  }

}
