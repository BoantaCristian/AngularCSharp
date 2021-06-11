import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { DisplayPaperComponent } from '../display-paper/display-paper.component';
import { PayComponent } from '../pay/pay.component';

@Component({
  selector: 'app-view-details-client',
  templateUrl: './view-details-client.component.html',
  styleUrls: ['./view-details-client.component.css']
})
export class ViewDetailsClientComponent implements OnInit {

  searchKey: string;
  waterProvider: any = []
  electricityProvider: any = []
  gasProvider: any = []
  associations: any = [];
  payments: any = []
  archives: any = []
  receipts: any = []

  displayProviderColumns: string[] = ["name", "location", "program", "coldWaterLiterPrice", "hotWaterLiterPrice", "electricityPrice", "gasPrice"]
  displayAssociationColumns: string[] = ["description", "location", "program", "workingCapital", "sanitation", "dayPenalty"]
  displayPaymentColumns: string[] = ["date", "totalDueWithPenalties", "remaining", "penalties", "daysDelay", "totalPaid", "workingCapitalStatus", "sanitationStatus", "paymentStatus", "actions"]
  displayArchiveColumns: string[] = ["date", "bathroom", "kitchen", "electricity", "gas", "totalPayment", "actions"]
  displayReceiptColumns: string[] = ["payDate", "amountPayed", "actions"]

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog, private service: AssociationService, private toastr: ToastrService) { }

  @ViewChild(MatSort, {static: false}) sort: MatSort //error solved stack overflow++
  @ViewChild('paymentsPaginator', {read: MatPaginator, static: false}) paymentsPaginator: MatPaginator;  
  @ViewChild('archivesPaginator', {read: MatPaginator, static: false}) archivesPaginator: MatPaginator;  
  @ViewChild('receiptsPaginator', {read: MatPaginator, static: false}) receiptsPaginator: MatPaginator;  

  ngOnInit() {
    this.getAssociationOfClient()
    this.getProviderOfClient()
    this.getArchivesOfClient()
    this.getPaymentOfClient()
    this.getReceiptsOfClient()
    setTimeout(()=> this.sortArrays(), 100)
  }

  getProviderOfClient(){
    this.service.getProviderOfClient(this.data.userId).subscribe(
      (res: any) => {
        this.waterProvider = res.waterProvider
        this.electricityProvider = res.electricityProvider
        this.gasProvider = res.gasProvider
      }
    )
  }

  getAssociationOfClient(){
    this.service.getAssociationOfClient(this.data.userId).subscribe(
      (res: any) => {
        this.associations = new MatTableDataSource(res)
      }
    )
  }
  getArchivesOfClient(){
    this.service.getArchivesOfClient(this.data.userId).subscribe(
      (res: any) => {
        this.archives = new MatTableDataSource(res)
      }
    )
  }
  
  getPaymentOfClient(){
    this.service.getPaymentOfClient(this.data.userId).subscribe(
      (res: any) => {
        this.payments = new MatTableDataSource(res)
      }
    )
  }
  getReceiptsOfClient(){
    this.service.getReceiptsOfClient(this.data.userId).subscribe(
      (res: any) => {
        this.receipts = new MatTableDataSource(res)
      }
    )
  }
  openDisplayPaperDialog(paper){
    var displayPaperDialog = this.dialog.open(DisplayPaperComponent, {data: paper})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
    })
  }
  openPayDialog(actions){
    var displayPaperDialog = this.dialog.open(PayComponent, {minWidth: "350px", data: {actions, Role: 'Client'}})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
      setTimeout(()=> this.getPaymentOfClient(), 100)
      setTimeout(()=> this.sortArrays(), 200)
    })
  }
  sortArrays(){
    this.payments.sort = this.sort
    this.archives.sort = this.sort
    this.receipts.sort = this.sort

    this.payments.paginator = this.paymentsPaginator
    this.archives.paginator = this.archivesPaginator
    this.receipts.paginator = this.receiptsPaginator
  }

  onSearchClear(){
    this.searchKey = ""
    this.applyFilter('')
  }

  applyFilter(option){
    switch (option){
      case 'archive':
        this.archives.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'receipt':
        this.receipts.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      default:
        break
    }
  }

}
