import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Sort, MatSort, MatTableDataSource, MAT_DIALOG_DATA, MatPaginator, MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';
import { DisplayPaperComponent } from '../display-paper/display-paper.component';

@Component({
  selector: 'app-view-details',
  templateUrl: './view-details.component.html',
  styleUrls: ['./view-details.component.css']
})
export class ViewDetailsComponent implements OnInit {
  admins: any = [];
  representatives: any = [];
  clients: any = [];
  providers: any = [];
  associations: any = [];
  payments: any = []
  archives: any = []
  receipts: any = []
  displayProviderColumns: string[] = ["name", "location", "program", "coldWaterLiterPrice", "hotWaterLiterPrice", "electricityPrice", "gasPrice", "actions"]
  displayAssociationColumns: string[] = ["description", "location", "program", "workingCapital", "sanitation", "dayPenalty", "actions"]
  displayAdminColumns: string[] = ["userName", "email", "address", "telephone", "actions"]
  displayRepresentativeColumns: string[] = ["userName", "email", "address", "telephone", "association", "clients", "actions"]
  displayClientColumns: string[] = ["userName", "email", "address", "telephone", "representative", "actions"]
  displayPaymentColumns: string[] = ["userName", "totalDueWithPenalties", "penalties", "daysDelay", "workingCapitalStatus", "sanitationStatus", "paymentStatus", "actions"]
  displayArchiveColumns: string[] = ["userName", "association", "date", "bathroom", "kitchen", "electricity", "gas", "totalPayment", "actions"]
  displayReceiptColumns: string[] = ["userName", "payDate", "amountPayed"]
  
  isLinear: boolean = false
  searchKey: string

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog, private service: AssociationService, private toastr: ToastrService) { }
  
  @ViewChild(MatSort, {static: false}) sort: MatSort //error solved stack overflow++

  @ViewChild('providersPaginator', {read: MatPaginator, static: false}) providersPaginator: MatPaginator;  
  @ViewChild('associationsPaginator', {read: MatPaginator, static: false}) associationsPaginator: MatPaginator;  
  @ViewChild('adminsPaginator', {read: MatPaginator, static: false}) adminsPaginator: MatPaginator;  
  @ViewChild('representativesPaginator', {read: MatPaginator, static: false}) representativesPaginator: MatPaginator;  
  @ViewChild('clientsPaginator', {read: MatPaginator, static: false}) clientsPaginator: MatPaginator;  
  @ViewChild('paymentsPaginator', {read: MatPaginator, static: false}) paymentsPaginator: MatPaginator;  
  @ViewChild('archivesPaginator', {read: MatPaginator, static: false}) archivesPaginator: MatPaginator;  
  @ViewChild('receiptsPaginator', {read: MatPaginator, static: false}) receiptsPaginator: MatPaginator;  


  ngOnInit() {
    this.getUsers()
    this.getAssociations()
    this.getProviders()
    this.getPayments()
    this.getArchives()
    this.getReceipts()
    setTimeout(()=> this.sortArrays(), 200)
    
  }

  getUsers(){
    this.service.getUsers().subscribe(
      (res: any) => {
        this.admins = new MatTableDataSource(res.admins)
        this.representatives = new MatTableDataSource(res.representatives)
        this.clients = new MatTableDataSource(res.clients)
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
  
  getPayments(){
    this.service.getPayment().subscribe(
      (res: any) => {
        this.payments = new MatTableDataSource(res)
      }
    )
  }
  
  getArchives(){
    this.service.getArchive().subscribe(
      (res: any) => {
        this.archives = new MatTableDataSource(res)
      }
    )
  }
  
  getReceipts(){
    this.service.getReceipts().subscribe(
      (res: any) => {
        this.receipts = new MatTableDataSource(res)
        console.log("receipts: ", res)
      }
    )
  }

  
  updatePayment(idPayment){
    this.service.updatePayment(idPayment).subscribe(
      res => {
        this.toastr.success('Payment and its penalties updated!', 'Success!')
      }
    )
  }
  
  deleteUser(userName){
    this.service.deleteUser(userName).subscribe(
      (res: any) => {
        this.toastr.success(`User ${res.userName} deleted successfully`, 'Success!')
        this.getUsers()
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`,'Failed!')
      }
    )
  }

  deleteProvider(idProvider){
    this.service.deleteProvider(idProvider).subscribe(
      (res: any) => {
        this.toastr.success(`Provider ${res.name} deleted successfully`, 'Success!')
        this.getProviders()
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`,'Failed!')
      }
    )
  }

  deleteAssociaton(idAssociation){
    console.log(idAssociation)
    this.service.deleteAssociation(idAssociation).subscribe(
      (res: any) => {
        this.toastr.success(`Association ${res.description} deleted successfully`, 'Success!')
        this.getAssociations()
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`,'Failed!')
      }
    )
  }

  openDisplayPaperDialog(paper){
    var displayPaperDialog = this.dialog.open(DisplayPaperComponent, {data: paper})
    displayPaperDialog.afterClosed().subscribe( (result: any) => {
    })
  }

  sortArrays(){
    
    this.providers.sort = this.sort
    this.associations.sort = this.sort
    this.admins.sort = this.sort
    this.representatives.sort = this.sort
    this.clients.sort = this.sort
    this.payments.sort = this.sort
    this.archives.sort = this.sort
    this.receipts.sort = this.sort

    this.providers.paginator = this.providersPaginator
    this.associations.paginator = this.associationsPaginator
    this.admins.paginator = this.adminsPaginator
    this.representatives.paginator = this.representativesPaginator
    this.clients.paginator = this.clientsPaginator
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
      case 'admin':
        this.admins.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'representative':
        this.representatives.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      case 'client':
        this.clients.filter = this.searchKey.trim().toLocaleLowerCase()
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
      case '':
        this.admins.filter = this.searchKey.trim().toLocaleLowerCase()
        this.representatives.filter = this.searchKey.trim().toLocaleLowerCase()
        this.clients.filter = this.searchKey.trim().toLocaleLowerCase()
        this.associations.filter = this.searchKey.trim().toLocaleLowerCase()
        this.providers.filter = this.searchKey.trim().toLocaleLowerCase()
        this.payments.filter = this.searchKey.trim().toLocaleLowerCase()
        this.archives.filter = this.searchKey.trim().toLocaleLowerCase()
        this.receipts.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      default:
        break
    }
  }

}
