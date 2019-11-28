import { Component, OnInit } from '@angular/core';
import { HighwayTollsService } from 'src/app/services/highway-tolls.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userDetails: any = {
    userName: '',
    email: '',
    role: ''
  };
  logged: boolean;
  locations: Object;
  tollBooths: Object;
  selectedLocationName: any;
  selectedLocationId: any;
  selectedTollBoothName: any;
  selectedTollBoothId: any;
  confirmPlace: boolean;
  categories: Object;
  price: any;
  selectedCategory: any;
  history: object
  income: Object;
  months: Object[] = [
    {monthNumber: 1, monthName:'January'},
    {monthNumber: 2, monthName:'February'},
    {monthNumber: 3, monthName:'March'},
    {monthNumber: 4, monthName:'April'},
    {monthNumber: 5, monthName:'May'},
    {monthNumber: 6, monthName:'June'},
    {monthNumber: 7, monthName:'July'},
    {monthNumber: 8, monthName:'August'},
    {monthNumber: 9, monthName:'September'},
    {monthNumber: 10, monthName:'October'},
    {monthNumber: 11, monthName:'November'},
    {monthNumber: 12, monthName:'December'}
  ]
  startLocation
  finishLocation
  vehicleCategory: any;
  routePrice: Object;

  constructor(private toastr: ToastrService, private router: Router ,private service: HighwayTollsService) { }

  ngOnInit() {
    this.getUser()
    this.getLocations()
    this.getCategories()
    this.getHistory()
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.logged = true
      this.service.getUser().subscribe(
        res => {
          this.userDetails = res
        },
        err => {
          console.log(err)
        }
      )
    }
    else{
      this.logged = false
    }
  }

  logout(){
    localStorage.removeItem('token')
    this.router.navigateByUrl('/user/login')
  }

  getLocations(){
    this.service.getLocations().subscribe(
      res => {
        this.locations = res
      }
    )
  }

  getCategories(){
    this.service.getCategories().subscribe(
      res => {
        this.categories = res
      }
    )
  }

  getHistory(){
    this.service.getHistory().subscribe(
      res => {
        this.history = res
      }
    )
  }

  getPrice(idCateg, nameCateg){
    this.selectedCategory = nameCateg
    this.service.getPrice(this.selectedLocationId, idCateg).subscribe(
      (res:any) => {
        this.price = res
      }
    )
  }

  selectLocation(id, loc){
    this.selectedLocationName = loc
    this.selectedLocationId = id
    this.service.getTollBooths(id).subscribe(
      (res:any) => {
        this.tollBooths = res
      }
    )
  }

  selectTollBooth(id, toll){
    this.selectedTollBoothName = toll
    this.selectedTollBoothId = id
  }

  confirmLocation(){
    this.confirmPlace = true
  }

  confirmPayment(){
    var body = {
      amount: this.price,
      date: new Date() //nu salveaza in db gmt +2 => diferenta de 2 ore fus orar; data e creata in controller
    }
    console.log(body.date)
    this.service.confirmPayment(this.userDetails.userName, this.selectedLocationId, body).subscribe(
      res=> {
        this.toastr.success(`Paid from ${this.selectedLocationName}: ${this.selectedTollBoothName} for ${this.selectedCategory} with ${this.price} RON by user ${this.userDetails.userName}`,'Payment succesful!')
        this.confirmPlace = false
        this.price = null
        this.selectedCategory = null
        this.selectedLocationId = null
        this.selectedLocationName = null
        this.selectedTollBoothId = null 
        this.selectedTollBoothName = null
        this.getHistory()
      }
    )
  }
  
  monthIncome(month){
    this.service.monthIncome(month).subscribe(
      res => {
        this.income = res
      }
    )
  }

  calculatePayment(){
    if(this.startLocation == this.finishLocation){
      this.toastr.error('The starting finish locations should be different.', 'Failed!')
    }
    else{
      this.service.calculatePriceRoute(this.startLocation, this.finishLocation, this.vehicleCategory).subscribe(
        res => {
          this.routePrice = res
          this.toastr.success(`The cost from ${this.startLocation} to ${this.finishLocation} for ${this.selectCategoryName} is ${res} RON`,'Successfull')
          this.startLocation = null 
          this.finishLocation = null 
          this.vehicleCategory = null
        }
      )
    }
  }
  selectCategoryName(categoryName){
    this.selectCategoryName = categoryName
  }
}
