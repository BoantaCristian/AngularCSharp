import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { AddAssociationComponent } from '../dialogs/add-association/add-association.component';
import { AddProviderComponent } from '../dialogs/add-provider/add-provider.component';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  
  constructor(public dialog: MatDialog, private fb: FormBuilder, private router: Router, private service: AssociationService, private toastr: ToastrService) { }
  
  adminDetails: any;
  associations: any;
  providers: any;
  users: any;

  ngOnInit() {
    // localStorage.removeItem('token')
    // localStorage.removeItem('role')
    this.getUser()
    this.getUsers()
    this.getAssociations()
    this.getProviders()
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.adminDetails = res
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
          this.router.navigateByUrl('')
        }
      )
    }
  }

  getUsers(){
    if(localStorage.getItem('token') != null){
      this.service.getUsers().subscribe(
        (res: any) => {
          this.users = res
        },
        err => {
          console.log(err)
        }
      )
    }
  }

  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    this.router.navigateByUrl('')
  }

  getAssociations(){
    this.service.getAssociations().subscribe(
      res => {
        this.associations = res
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

  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent, {data: 'Admin'})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { this.getUsers() }, 300);
    })
  }

  openAddAssociationDialog(){
    var associationDialog = this.dialog.open(AddAssociationComponent)
    associationDialog.afterClosed().subscribe( result => {
      if(result != undefined){
        this.toastr.success('Association added successfully', 'Success!')
      }
      setTimeout(() => this.getAssociations(), 300)
    })
  }
  openAddProviderDialog(){
    var providerDialog = this.dialog.open(AddProviderComponent)
    providerDialog.afterClosed().subscribe( result => {
      if(result != undefined){
        this.toastr.success('Provider added successfully', 'Success!')
      }
      setTimeout(() => this.getProviders(), 300)
    })
  }
}
