import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private form: FormBuilder, private service: AccidentService, private router: Router, private toastr: ToastrService) { }

  loginForm = this.form.group({
    UserName: ['', Validators.required],
    Password: ['', Validators.required]
  })

  ngOnInit() {
  }

  login(){
    this.service.login(this.loginForm.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token)
        this.router.navigateByUrl('')
      },
      err => {
        if(err.status == 400){
          this.toastr.error(`Incorrect username or password`, 'Falied!')
        }
      }
    )
  }

}
