import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { AssociationService } from 'src/app/services/association.service';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-representative',
  templateUrl: './representative.component.html',
  styleUrls: ['./representative.component.css']
})
export class RepresentativeComponent implements OnInit {
  representativeDetails: (any) = {
    id: ''
  };

  constructor(private router: Router, public dialog: MatDialog, private service: AssociationService) { }

  ngOnInit() {
    this.getUser()
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.representativeDetails = res
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
          this.router.navigateByUrl('')
        }
      )
    }
  }

  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    this.router.navigateByUrl('')
  }
  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent, {data: this.representativeDetails.id})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { }, 300);
    })
  }

}
