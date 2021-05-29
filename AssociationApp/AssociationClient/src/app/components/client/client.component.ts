import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { AssociationService } from 'src/app/services/association.service';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {
  clientDetails: any;

  constructor(private router: Router, public dialog: MatDialog, private service: AssociationService) { }

  ngOnInit() {
    this.getUser()
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.clientDetails = res
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

}
