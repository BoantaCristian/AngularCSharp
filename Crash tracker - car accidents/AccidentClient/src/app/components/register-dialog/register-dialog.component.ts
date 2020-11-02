import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-register-dialog',
  templateUrl: './register-dialog.component.html',
  styleUrls: ['./register-dialog.component.css']
})
export class RegisterDialogComponent implements OnInit {

  
  mismatch: boolean;
  roles = ['Admin', 'Supervizor', 'Agent']
  needSupervizor: boolean = false;
  selectedSupervisor: boolean = false;
  supervisors: Object;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private form: FormBuilder, private service: AccidentService, private toastr: ToastrService) { }

  registerForm = this.form.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['', Validators.required],
    Supervizor: ''
  })

  ngOnInit() {
    this.getSupervisors()
  }

  register(){
    var body = {
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      Password: this.registerForm.value.Password,
      Role: this.registerForm.value.Role,
      Supervizor: this.registerForm.value.Supervizor
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

  selectSupervizor(role){
    if(role == 'Agent'){
      this.needSupervizor = true
    }
    else
      this.needSupervizor = false
  }
  supervisorSelected(){
    this.selectedSupervisor = true
  }

  getSupervisors(){
    this.service.getSupervisors().subscribe(
      (res: any) => {
        this.supervisors = res
      }
    )
  }

}
