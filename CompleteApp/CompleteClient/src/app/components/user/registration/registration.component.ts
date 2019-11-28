import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private fb: FormBuilder, private service: UserService, private toastr: ToastrService) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, {validator: this.comparePasswords})
  })

  ngOnInit() {
    this.formModel.reset()
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

  onSubmit(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Passwords.Password
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.formModel.reset()
          this.toastr.success('New user created', 'Registration successfull')
        } else {
          res.errors.forEach(element => {
            switch (element.code){
              case 'DuplicateUserName':
                this.toastr.error('Username taken', 'Registration failed')
                break
              default:
                  this.toastr.error(element.description, 'Registration failed')
                break
            }
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }

}
