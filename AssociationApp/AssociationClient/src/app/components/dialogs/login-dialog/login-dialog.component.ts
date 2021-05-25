import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent implements OnInit {

  constructor(private fb: FormBuilder, private service: AssociationService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }

  loginForm = this.fb.group({
    UserName: ['', Validators.required],
    Password: ['', [Validators.required, Validators.minLength(4)]]
  })

  login(){
    this.service.login(this.loginForm.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token)
        this.getUser()
        setTimeout(() => this.navigateToSpecificPage(), 100);
        setTimeout(() => this.toastr.success(`Login Successful!`), 200);
      },
      err => {
        if(err.status == 400){
          this.toastr.error(`Incorrect username or password`, 'Falied!')
        }
      }
    )
  }

  navigateToSpecificPage(){
    if(localStorage.getItem('role') == 'Admin')
      this.router.navigateByUrl('admin')
    if(localStorage.getItem('role') == 'Client')
      this.router.navigateByUrl('user/client')
    if(localStorage.getItem('role') != 'Admin' && localStorage.getItem('role') != 'Client')
      this.router.navigateByUrl('user/representative')
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          localStorage.setItem('role', res.role)
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
          this.router.navigateByUrl('')
        }
      )
    }
  }

}
