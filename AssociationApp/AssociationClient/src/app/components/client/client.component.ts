import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { EmitPaymentComponent } from '../dialogs/emit-payment/emit-payment.component';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';
import { ViewDetailsClientComponent } from '../dialogs/view-details-client/view-details-client.component';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {
  clientDetails: any = {representative:{representative:{association: {}}}};

  constructor(private router: Router, public dialog: MatDialog, private service: AssociationService, private toastr: ToastrService) { }

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

  openEmitPaymentDialog(){
    var paymentDialog = this.dialog.open(EmitPaymentComponent, {data: 'Client'})
    paymentDialog.afterClosed().subscribe( (result: any) => {
      this.service.updatePayments().subscribe(
        res => {
          this.toastr.success('Payments and penalties updated!', 'Success!')
        }
      )
    })
  }
  openViewDetailsDialog(option){
    var registerDialog = this.dialog.open(ViewDetailsClientComponent, {width: '1200px', minWidth: '1200px', data: {option, userId: this.clientDetails.id}})
    registerDialog.afterClosed().subscribe( () =>{ })
  }

}
