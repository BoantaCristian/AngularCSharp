import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { HighwayTollsService } from 'src/app/services/highway-tolls.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private fb: FormBuilder, private service: HighwayTollsService, private toastr: ToastrService) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role:['Client']
  })

  passwordsMatch

  ngOnInit() {
  }

  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role
    }
    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} successfully created`,'Registration succeeded')
          this.formModel.reset()
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
      err => {
        console.log(err)
      }
    )
  }
  comparePasswords(){
    if(this.formModel.value.Password === this.formModel.value.ConfirmPassword){
      this.passwordsMatch = true
    }
    else{
      this.passwordsMatch = false
    }
  }

}
