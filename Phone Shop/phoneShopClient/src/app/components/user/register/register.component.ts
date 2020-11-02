import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private router: Router, private fb: FormBuilder, private service: PhoneShopService, private toastr: ToastrService) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: 'Client'
  })

  passwordsMatch:Boolean = false

  ngOnInit() {
    this.userLogged()
  }
  userLogged(){
    if(localStorage.getItem('token'))
      this.router.navigateByUrl('')
  }
  comparePasswords(){
    if(this.formModel.value.Password == this.formModel.value.ConfirmPassword){
      this.passwordsMatch = true
    }
    else
      this.passwordsMatch = false
  }
  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role,
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.toastr.success(`User ${this.formModel.value.UserName} added!`,'Successfully registered!')
          this.formModel.reset()
          this.formModel.markAsUntouched()
        }
        else{
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName'){
              this.toastr.error('Username taken','Registration failed!')
            }
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
        console.log(res)
      }
    )
  }

}
