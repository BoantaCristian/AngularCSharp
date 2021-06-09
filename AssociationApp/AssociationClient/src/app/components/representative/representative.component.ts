import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { EmitPaymentComponent } from '../dialogs/emit-payment/emit-payment.component';
import { RegisterDialogComponent } from '../dialogs/register-dialog/register-dialog.component';
import { ViewDetailsRepresentativeComponent } from '../dialogs/view-details-representative/view-details-representative.component';
import { ViewDetailsComponent } from '../dialogs/view-details/view-details.component';

@Component({
  selector: 'app-representative',
  templateUrl: './representative.component.html',
  styleUrls: ['./representative.component.css']
})
export class RepresentativeComponent implements OnInit {
  representativeDetails: any = { };

  constructor(private router: Router, public dialog: MatDialog, private service: AssociationService, private toastr: ToastrService) { }

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
  
  updatePayments(){
    this.service.updatePaymentsOfRepresentative(this.representativeDetails.id).subscribe(
      res => {
        this.toastr.success('Payments and penalties updated!', 'Success!')
      }
    )
  }
  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent, {data: this.representativeDetails.id})
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => { }, 300);
    })
  }
  openViewDetailsDialog(option){
    var registerDialog = this.dialog.open(ViewDetailsRepresentativeComponent, {width: '1500px', minWidth: '1500px', data: {option, userId: this.representativeDetails.id}})
    registerDialog.afterClosed().subscribe( () =>{ })
  }
  openEmitPaymentDialog(){
    var paymentDialog = this.dialog.open(EmitPaymentComponent, {data: 'Representative'})
    paymentDialog.afterClosed().subscribe( (result: any) => {
      this.updatePayments()
    })
  }

}
