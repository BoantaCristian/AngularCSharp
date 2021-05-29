import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Sort, MatSort, MatTableDataSource, MAT_DIALOG_DATA, MatPaginator } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

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
  displayProviderColumns: string[] = ["name", "location", "program", "coldWaterLiterPrice", "hotWaterLiterPrice", "electricityPrice", "gasPrice", "actions"]
  displayAssociationColumns: string[] = ["description", "location", "program", "workingCapital", "sanitation", "dayPenalty", "actions"]
  displayAdminColumns: string[] = ["userName", "email", "address", "telephone", "actions"]
  displayRepresentativeColumns: string[] = ["userName", "email", "address", "telephone", "association", "clients", "actions"]
  displayClientColumns: string[] = ["userName", "email", "address", "telephone", "representative", "actions"]
  isLinear: boolean = false
  searchKey: string

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private service: AssociationService, private toastr: ToastrService) { }
  
  @ViewChild(MatSort, {static: false}) sort: MatSort //error solved stack overflow++
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator //error solved stack overflow++
  
  ngOnInit() {
    this.getUsers()
    this.getAssociations()
    this.getProviders()
    setTimeout(()=> this.sortArrays(), 200)
    
  }

  getUsers(){
    this.service.getUsers().subscribe(
      (res: any) => {
        this.admins = new MatTableDataSource(res.admins)
        this.representatives = new MatTableDataSource(res.representatives)
        this.clients = new MatTableDataSource(res.clients)

        console.log("admins", res.admins)
        console.log("rep", res.representatives)
        console.log("clients", res.clients)
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

  sortArrays(){
    
    this.providers.sort = this.sort
    this.associations.sort = this.sort
    this.admins.sort = this.sort
    this.representatives.sort = this.sort
    this.clients.sort = this.sort

    this.providers.paginator = this.paginator
    this.associations.paginator = this.paginator
    this.admins.paginator = this.paginator
    this.representatives.paginator = this.paginator
    this.clients.paginator = this.paginator
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
      case '':
        this.admins.filter = this.searchKey.trim().toLocaleLowerCase()
        this.representatives.filter = this.searchKey.trim().toLocaleLowerCase()
        this.clients.filter = this.searchKey.trim().toLocaleLowerCase()
        this.associations.filter = this.searchKey.trim().toLocaleLowerCase()
        this.providers.filter = this.searchKey.trim().toLocaleLowerCase()
        break
      default:
        break
    }
  }

}
