import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MedicalOfficeService } from 'src/app/services/medical-office.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private router:Router, private fb:FormBuilder, private toastr: ToastrService, private service: MedicalOfficeService) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Address: '',
    PhoneNumber: [''],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['Pacient'],
    IdDoctor: ['', Validators.required],
  })

  mismatch = false
  
  doctors

  ngOnInit() {
    this.getDoctors()
    this.checkUser()
  }
  getDoctors(){
    this.service.getDoctors().subscribe(
      (res) => {
        this.doctors = res
      }
    )
  }
  checkUser(){
    if(localStorage.getItem('token') != null)
      this.router.navigateByUrl('')
  }
  comparePasswords(){
    if(this.formModel.value.Password != this.formModel.value.ConfirmPassword){
      this.mismatch = true
    }
    else{
      this.mismatch = false
    }
  }

  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Address: this.formModel.value.Address,
      PhoneNumber: this.formModel.value.PhoneNumber,
      Role: 'Pacient',//this.formModel.value.Role
      IdDoctor: this.formModel.value.IdDoctor
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

}
