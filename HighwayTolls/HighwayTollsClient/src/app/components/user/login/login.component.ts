import { Component, OnInit } from '@angular/core';
import { HighwayTollsService } from 'src/app/services/highway-tolls.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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

  constructor(private toastr: ToastrService, private router: Router, private service: HighwayTollsService) { }

  ngOnInit() {
    if(localStorage.getItem('token') != null)
      this.router.navigateByUrl('')
  }

  login(){
    this.service.login(this.formModel).subscribe(
      (res:any) => {
        localStorage.setItem('token', res.token)
        this.router.navigateByUrl('')
      },
      err => {
        if(err.status == 400)
          this.toastr.error('Incorrect username or password','Authentication failed')
      }
    )
  }

}
