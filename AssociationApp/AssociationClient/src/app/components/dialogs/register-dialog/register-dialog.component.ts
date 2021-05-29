import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-register-dialog',
  templateUrl: './register-dialog.component.html',
  styleUrls: ['./register-dialog.component.css']
})
export class RegisterDialogComponent implements OnInit {
  
  
  
  constructor(@Inject(MAT_DIALOG_DATA) public role: any, private router: Router, private fb: FormBuilder, private service: AssociationService, private toastr: ToastrService) { }
  
  registerForm = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    CNP: ['', [Validators.required, Validators.minLength(13)]],
    Address: '',
    Telephone: '',
    AssociationId: 0,
    RepresentativeId: '',
    WaterProvider: 0,
    GasProvider: 0,
    ElectricityProvider: 0,
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: '',
    
  })
  
  roleSelectedByAdmin: any = '';
  mismatch = false
  roles = ['Admin', 'Representative', 'Client']
  representatives: any = []
  associations: any = [ ]
  providers: any;
  admins: any;
  clients: any;
  hide = true;
  
  ngOnInit() {
    this.checkRole()
    this.getUsers()
    this.getAssociations()
    this.getProviders()
  }
  checkRole(){
    if(this.role != 'Admin' && this.role != 'Client'){
      this.registerForm.setValue({
        UserName: this.registerForm.value.UserName,
        Email: this.registerForm.value.Email,
        CNP: this.registerForm.value.CNP,
        Address: this.registerForm.value.Address,
        Telephone: this.registerForm.value.Telephone,
        AssociationId: 0,
        RepresentativeId: this.role,
        WaterProvider: 0,
        GasProvider: 0,
        ElectricityProvider: 0,
        Password:  this.registerForm.value.Password,
        ConfirmPassword: this.registerForm.value.ConfirmPassword,
        Role: this.registerForm.value.Role,
      })
      if(this.role == ''){
        this.toastr.error('Id of representative missing', 'Error')
      }
    }
  }
  matchPasswords(){
    if(this.registerForm.value.Password == this.registerForm.value.ConfirmPassword)
      this.mismatch = false
    else
      this.mismatch = true
  }
  selectRole(role){
    console.log(role)
    this.roleSelectedByAdmin = role
    this.registerForm.reset({
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      CNP: this.registerForm.value.CNP,
      Address: this.registerForm.value.Address,
      Telephone: this.registerForm.value.Telephone,
      AssociationId: 0,
      RepresentativeId: '',
      WaterProvider: 0,
      GasProvider: 0,
      ElectricityProvider: 0,
      Password:  this.registerForm.value.Password,
      ConfirmPassword: this.registerForm.value.ConfirmPassword,
      Role: this.registerForm.value.Role,
    })
  }

  register(){
    var body = {
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      CNP: this.registerForm.value.CNP,
      Address: this.registerForm.value.Address,
      Telephone: this.registerForm.value.Telephone,
      AssociationId: this.registerForm.value.AssociationId,
      RepresentativeId: this.registerForm.value.RepresentativeId,
      WaterProvider: this.registerForm.value.WaterProvider,
      GasProvider: this.registerForm.value.GasProvider,
      ElectricityProvider: this.registerForm.value.ElectricityProvider,
      Password:  this.registerForm.value.Password,
      Role: '',
    }
    if(this.role != 'Admin' && this.role != 'Client'){
      body.Role = 'Client'
      body.RepresentativeId = this.role
    }
    else
      body.Role = this.roleSelectedByAdmin
      
    console.log(body)

    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} created successfully`,'Success!')
          this.registerForm.reset()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username already taken', 'Register Failed!')
            else
              this.toastr.error(`${element.description}`, 'Register Failed!')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }

  getUsers(){
    this.service.getUsers().subscribe(
      (res: any) => {
        this.admins = res
        this.representatives = res.representatives
        this.clients = res
      }
    )
  }
  getProviders(){
    this.service.getProviders().subscribe(
      res => {
        this.providers = res
      }
    )
  }

  getAssociations(){
    this.service.getAssociations().subscribe(
      res => {
        this.associations = res
      }
    )
  }


}
