import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-representative',
  templateUrl: './representative.component.html',
  styleUrls: ['./representative.component.css']
})
export class RepresentativeComponent implements OnInit {
  userDetails: (any) = {
    id: ''
  };

  constructor(public dialog:MatDialog) { }

  ngOnInit() {
  }
  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent, {data: this.userDetails.id})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { }, 300);
    })
  }

}
