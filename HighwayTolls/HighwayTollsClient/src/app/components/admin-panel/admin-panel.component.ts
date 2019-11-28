import { Component, OnInit } from '@angular/core';
import { HighwayTollsService } from 'src/app/services/highway-tolls.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  users: Object;
  roles: Object;
  newRoles = ['Admin', 'Client']
  

  constructor(private service: HighwayTollsService, private fb:FormBuilder, private toastr: ToastrService) { }
  formModel = this.fb.group( {
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    Role: ['', Validators.required]
  })
  ngOnInit() {
    this.getUsers()
    this.getUserRoles()
  }

  getUsers(){
    this.service.getUsers().subscribe(
      (res:any) =>{
        this.users = res
      }
    )
  }
  getUserRoles(){
    this.service.getUserRoles().subscribe(
      (res:any) => {
        this.roles = res
      }
    )
  }
  deleteUser(userName){
    this.service.deleteUser(userName).subscribe(
      res => {
        this.getUsers()
        this.getUserRoles()
      }
    )
  }
  selectRole(role){
    this.formModel.value.Role = role
  }
  resetForm(){
    this.formModel.reset()
  }
  register(){
    var body ={
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role,
    }
    this.service.registerByAdmin(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.formModel.reset()
          this.toastr.success(`User ${body.UserName} successfully created`,'Registration succeeded')
          this.getUsers()
          this.getUserRoles()
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
