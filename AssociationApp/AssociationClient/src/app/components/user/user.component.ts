import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { LoginDialogComponent } from '../dialogs/login-dialog/login-dialog.component';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(public dialog:MatDialog) { }

  ngOnInit() {
  }

  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent, { data: 'Client'})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { }, 300);
    })
  }
  openLoginDialog(){
    var registerDialog = this.dialog.open(LoginDialogComponent, {width: '400px'})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { }, 300);
    })
  }

}
