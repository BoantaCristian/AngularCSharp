import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MedicalService } from 'src/app/services/medical.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private router: Router, private fb: FormBuilder, private service: MedicalService, private toastr: ToastrService) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    PhoneNumber: [''],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['Medic'],
    Seniority: ['', Validators.required]
  })
  mismatch = false
  
  ngOnInit() {
    this.checkUser()
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
      PhoneNumber: this.formModel.value.PhoneNumber,
      Password: this.formModel.value.Password,
      Role: 'Medic',//this.formModel.value.Role
      Seniority: this.formModel.value.Seniority
    }
    if(this.formModel.value.Seniority < 1)
    {
      this.toastr.error('Vechimea trebuie sa fie mai mare decat 0', 'Inregistrare esuata!')
      return 0;
    }
    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`Utilizatorul ${body.UserName} a fost creat cu succes`,'Inregistrat cu succes!')
          this.formModel.reset()
          //this.formModel.value.Role = 'Medic' 
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username existent', 'Inregistrare esuata!')
            else
              this.toastr.error(`${element.description}`, 'Inregistrare esuata!')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }

}
