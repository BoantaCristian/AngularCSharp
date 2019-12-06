import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MountainZoneService } from 'src/app/services/mountain-zone.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  passworsMatch
  
  constructor(private router: Router, private toastr: ToastrService, private fb: FormBuilder, private service: MountainZoneService) { }
  
  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role:['Client']
  })

  ngOnInit() {
    if(localStorage.getItem('token') != null){
      this.router.navigateByUrl('')
    }
  }

  comparePasswords(){
    if(this.formModel.value.Password == this.formModel.value.ConfirmPassword){
      this.passworsMatch = true
    }
    else
      this.passworsMatch = false
  }

  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role
    }
    this.service.register(body).subscribe(
      res => {
        this.formModel.reset()
        this.toastr.success('Success','Success')
      }
    )
  }

}
