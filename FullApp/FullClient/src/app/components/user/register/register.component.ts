import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { UserService } from "../../../services/user.service";
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private router: Router, private toastr: ToastrService, private fb:FormBuilder, private service:UserService) { }

  roles = ['Admin', 'Customer']

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Role: [''],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required]
  }, {validator: this.comparePasswords} )

  ngOnInit() {
    if(localStorage.getItem('token') != null){
      this.router.navigateByUrl('')
    }
  }

  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.formModel.reset()
          this.toastr.success('New user created','Registration successful')
        }
        else{
          res.errors.forEach(element => {
            if(element.code == "DuplicateUserName"){
              this.toastr.error('Username already taken','Registration failed')
            }
            else
              this.toastr.error(element.description,'Register failed')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }

  comparePasswords(fb: FormGroup){
    let confirmPass = fb.get('ConfirmPassword')
    if(confirmPass.errors == null || 'passwordMismatch' in confirmPass.errors){
      if(fb.get('Password').value != confirmPass.value){
        confirmPass.setErrors({passwordMismatch: true})
      }
      else 
        confirmPass.setErrors(null)
    }
  }

}
