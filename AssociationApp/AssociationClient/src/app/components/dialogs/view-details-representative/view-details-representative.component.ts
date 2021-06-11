import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { AddReceiptPaperComponent } from '../add-receipt-paper/add-receipt-paper.component';
import { DisplayPaperComponent } from '../display-paper/display-paper.component';
import { PayComponent } from '../pay/pay.component';

@Component({
  selector: 'app-view-details-representative',
  templateUrl: './view-details-representative.component.html',
  styleUrls: ['./view-details-representative.component.css']
})
export class ViewDetailsRepresentativeComponent implements OnInit {
  clientsOfRepresentative: any = [];
  searchKey: string;
  providers: any = [];
  associations: any = [];
  payments: any = []
  archives: any = []
  receipts: any = []
  
  displayClientsOfRepresentativeColumns: string[] = ["userName", "email", "address", "telephone", "actions"]
  displayProviderColumns: string[] = ["name", "location", "program", "coldWaterLiterPrice", "hotWaterLiterPrice", "electricityPrice", "gasPrice"]
  displayAssociationColumns: string[] = ["description", "location", "program", "workingCapital", "sanitation", "dayPenalty"]
  displayPaymentColumns: string[] = ["userName", "date", "totalDueWithPenalties", "remaining", "penalties", "daysDelay", "totalPaid", "workingCapitalStatus", "sanitationStatus", "paymentStatus", "actions"]
  displayArchiveColumns: string[] = ["userName", "date", "bathroom", "kitchen", "electricity", "gas", "totalPayment", "actions"]
  displayReceiptColumns: string[] = ["ReceiptClient", "payDate", "amountPayed", "actions"]

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog, private service: AssociationService, private toastr: ToastrService) { }
 
  @ViewChild(MatSort, {static: false}) sort: MatSort //error solved stack overflow++
  @ViewChild('clientsOfRepresentativePagiantor', {read: MatPaginator, static: false}) clientsOfRepresentativePagiantor: MatPaginator;  
  @ViewChild('providersPaginator', {read: MatPaginator, static: false}) providersPaginator: MatPaginator;  
  @ViewChild('associationsPaginator', {read: MatPaginator, static: false}) associationsPaginator: MatPaginator;  
  @ViewChild('paymentsPaginator', {read: MatPaginator, static: false}) paymentsPaginator: MatPaginator;  
  @ViewChild('archivesPaginator', {read: MatPaginator, static: false}) archivesPaginator: MatPaginator;  
  @ViewChild('receiptsPaginator', {read: MatPaginator, static: false}) receiptsPaginator: MatPaginator;  

  ngOnInit() {
    this.getClientsOfRepresentative()
    this.getProviders()
    this.getAssociations()
    this.getPaymentsOfRepresentative()
    this.getArchivesOfRepresentative()
    this.getReceiptsOfRepresentative()
    setTimeout(()=> this.sortArrays(), 100)
  }

  getClientsOfRepresentative(){
    this.service.getClientsOfRepresentative(this.data.userId).subscribe(
      (res: any) => {
        this.clientsOfRepresentative = new MatTableDataSource(res)
      }
    )
  }

  getProviders(){
    this.service.getProviders().subscribe(
      (res: any) => {
        this.providers = new MatTableDataSource(res)
      }
    )
  }

  getAssociations(){
    this.service.getAssociations().subscribe(
      (res: any) => {
        this.associations = new MatTableDataSource(res)
      }
    )
  }
  getArchivesOfRepresentative(){
    this.service.getArchiveOfRepresentative(this.data.userId).subscribe(
      (res: any) => {
        this.archives = new MatTableDataSource(res)
      }
    )
  }
  
  getPaymentsOfRepresentative(){
    this.service.getPaymentOfRepresentative(this.data.userId).subscribe(
      (res: any) => {
        this.payments = new MatTableDataSource(res)
      }
    )
  }
  getReceiptsOfRepresentative(){
    this.service.getReceiptsOfRepresentative(this.data.userId).subscribe(
      (res: any) => {
        this.receipts = new MatTableDataSource(res)
      }
    )
  }
  deletePayment(idPayment){
    this.service.deletePayment(idPayment).subscribe(
      res => {
        this.toastr.success('Payment deleted successfully', 'Success!')
        this.getPaymentsOfRepresentative()
        setTimeout(()=> this.sortArrays(), 100)
      },
      err => {
        this.toastr.error(`${err.error.message}`, 'Failed!')
      }
    )
  }
  openPayDialog(actions){
    var displayPaperDialog = this.dialog.open(PayComponent, {minWidth: "350px", data: {actions, Role: 'Admin'}})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
      setTimeout(()=> this.getPaymentsOfRepresentative(), 100)
      setTimeout(()=> this.sortArrays(), 200)
    })
  }
  openDisplayPaperDialog(paper){
    var displayPaperDialog = this.dialog.open(DisplayPaperComponent, {data: paper})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
    })
  }
  
  openAddReceiptPaperDialog(paper){
    var displayPaperDialog = this.dialog.open(AddReceiptPaperComponent, {data: paper})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
      setTimeout(()=> this.getReceiptsOfRepresentative(), 100)
      setTimeout(()=> this.sortArrays(), 200)
    })
  }
  
  sortArrays(){
    this.clientsOfRepresentative.sort = this.sort
    this.providers.sort = this.sort
    this.associations.sort = this.sort
    this.payments.sort = this.sort
    this.archives.sort = this.sort
    this.receipts.sort = this.sort

    this.clientsOfRepresentative.paginator = this.clientsOfRepresentativePagiantor
    this.providers.paginator = this.providersPaginator
    this.associations.paginator = this.associationsPaginator
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
      case 'clientOfRepresentative':
        this.clientsOfRepresentative.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'association':
        this.associations.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'provider':
        this.providers.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'payment':
        this.payments.filter = this.searchKey.trim().toLocaleLowerCase()
        break
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
