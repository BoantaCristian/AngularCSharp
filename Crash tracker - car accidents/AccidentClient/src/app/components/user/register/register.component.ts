import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  mismatch: boolean;

  constructor(private form: FormBuilder, private service: AccidentService, private toastr: ToastrService) { }

  registerForm = this.form.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: 'Agent'
  })

  ngOnInit() {
  }

  register(){
    var body = {
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      Password: this.registerForm.value.Password,
      Role: 'Admin'
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} registered with success`, "Success!")
          this.registerForm.reset()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username taken', 'Registration failed!')
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
      },
      err =>{
        console.log(err)
      }
    )
  }

  matchPasswords(){
    if(this.registerForm.value.Password == this.registerForm.value.ConfirmPassword)
      this.mismatch = false
    else
      this.mismatch = true
  }

}
