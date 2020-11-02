import { Component, OnInit } from '@angular/core';
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { Router } from '@angular/router';
import { MatDialog, TransitionCheckState } from '@angular/material';
import { DetailDialogComponent } from '../detail-dialog/detail-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import {map, startWith} from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userDetails: object = {
    username: '',
    email: '',
    role: ''
  };
  admin: Boolean
  logged: boolean;
  phones: any
  bagNumber: Object;
  currentUsername: any;
  currentPhoneQuantity = 1
  userName: any;

  peopleToFilter: any
  searchName: ''
  companies: Object;
  companyToSearch: string;

  features: string[] = ['Dimensions', 'Weight', 'Display Type', 'Resolution', 'OS', 'Main Camera', 'Selfie Camera', 'Battery']
  
  searchOptions: string[] = ['Samsung', 'Galaxy', 'S9', 'S10', 'S10+', 'Samsung Galaxy', 'Samsung S9', 'Samsung S10', 'Samsung S10+', 'Huawei', 'P30', 'Huawei P30', 'Huawei P30 Pro', 'P30Pro', 'Iphone','Iphone 11', 'Iphone 11 Pro', 'Iphone X', 'Pro']
  filteredOptions: Observable<string[]>;
  myControl:FormControl = new FormControl();
  searchProduct: Boolean = true
  selectedFeature: string = '';

  constructor(private toastr: ToastrService, private service: PhoneShopService, private router: Router, public dialog:MatDialog) { }

  ngOnInit() {
    this.checkUser()
    this.getPhones()
    this.getCompanies()
    setTimeout(()=> this.checkBag(), 100)

    this.filteredOptions = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.searchOptions.filter(option => option.toLowerCase().includes(filterValue));
  }

  checkUser(){
    if(localStorage.getItem('token')){
      this.service.getUser().subscribe(
        (res: any)=>{
          this.userDetails = res
          this.userName = res.userName
          this.currentUsername = res.userName
          this.logged = true
          localStorage.setItem('role', res.role)
          if(res.email == ''){
            res.email = 'no email'
          }
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
        }
      )
    }
    else this.logged = false
    if(localStorage.getItem('role') == 'Admin'){
      this.admin = true
    }
    else 
      this.admin = false
  }
  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    this.router.navigateByUrl('/user/login')
  }
  openDialog(idPhone){
    this.dialog.open(DetailDialogComponent, {data: idPhone})
  }
  checkBag(){
    var username = this.currentUsername
    this.service.checkBag(username).subscribe(
      res =>{
        this.bagNumber = res;
      }
    )
  }
  getPhones(){
    this.service.getPhonesWithCompanies().subscribe(
      res => {
        this.phones = res
      }
    )
  }
  
  getCompanies(){
    this.service.getCompanies().subscribe(
      res => {
        this.companies = res
      }
    )
  }
  addToBag(phoneId){
    var body = {
      Quantity: this.currentPhoneQuantity
    }
    this.service.addToBag(this.userName, phoneId, body).subscribe(
      res => {
        this.currentPhoneQuantity = 1
        this.checkBag()
        this.toastr.success('Phone successfully added in cart!')
      },
      (err: any) =>{
        console.log(err.error.message)
        this.toastr.error(`${err.error.message}`, 'Add to cart failed')
        this.currentPhoneQuantity = 1
      }
    )
  }
  manage(){
    if(localStorage.getItem('role') == 'Admin')
      this.router.navigateByUrl('admin')
  }
  Search(){
    if(this.searchName == ""){
      this.getPhones()
    }
    else if(this.searchProduct) 
      this.phones = this.phones.filter( res => {
        return res.name.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
      })
    else if(!this.searchProduct){
      switch(this.selectedFeature){
        case 'Dimensions':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].dimensions.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Weight':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].weight.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Display Type':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].displayType.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Resolution':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].resolution.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'OS':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].os.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Main Camera':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].mainCamera.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Selfie Camera':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].selfieCamera.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        case 'Battery':
          this.phones = this.phones.filter( res => {
            return res.descriptions[0].battery.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
          })
          break;
        default: break;
      }
    }
  }
  searchMode(){
    if(this.searchProduct)
      this.searchProduct = false
    else
      this.searchProduct = true
  }
  resetFeature(){
    this.selectedFeature = ''
    this.searchName = ''
    this.getPhones()
  }
  filterCompany(company){
    if(company == "All")
      this.getPhones()
    else{
      this.getPhones()
      setTimeout(()=> this.phones = this.phones.filter( res => {
        return res.company.name.toLocaleLowerCase().match(company.toLocaleLowerCase())
      }), 100)
    } 
  }
}
