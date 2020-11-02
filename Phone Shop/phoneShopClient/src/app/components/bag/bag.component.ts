import { Component, OnInit } from '@angular/core';
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { FinishDailogOrderComponent } from '../finish-dailog-order/finish-dailog-order.component';

@Component({
  selector: 'app-bag',
  templateUrl: './bag.component.html',
  styleUrls: ['./bag.component.css']
})
export class BagComponent implements OnInit {
  shoppingBag: any
  userName: any;
  total = 0;
  emptyShoppingBag: boolean = true;

  constructor(private dialog: MatDialog, private router: Router, private service:PhoneShopService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getUser()
    setTimeout(() => this.getBag(), 200);
    setTimeout(() => this.calculateTotal(), 400);
    
  }
  getBag(){
    this.service.getBag(this.userName).subscribe(
      (res: any) =>{
        this.shoppingBag = res
        if(res == '')
          this.emptyShoppingBag = true
        else
        this.emptyShoppingBag = false
      }
    )
  }
  getUser(){
    this.service.getUser().subscribe(
      (res: any) =>{
        this.userName = res.userName
      }
    )
  }
  calculateTotal(){
    for(const element of this.shoppingBag){
      this.total = this.total + element.quantity * element.price
    }
  }
  removeFromBag(idPhone){
    this.service.deletePhoneFromBag(idPhone).subscribe(
      (res: any) => {
        this.getBag()
        this.total = 0
        setTimeout(()=> this.calculateTotal(), 100)
        this.toastr.success('Phone removed from shopping bag', 'Remove successful!')
      },
      err => {
        console.log(err)
      }
    )
  }
  editQuantity(idBagElement, quantity, userName, phoneId){
    if(quantity <= 0){
      this.toastr.error('Quantity can\'t be lower than 1, press remove if you don\'t want the phone anymore', 'Incorrect quantity!')
      return 0
    }
    var body = {
      Id: idBagElement,
      Quantity: quantity
    }
    this.service.editQuantity(phoneId, userName, body).subscribe(
      res=>{
        this.getBag()
        this.total = 0
        setTimeout(()=> this.calculateTotal(), 100)
        this.toastr.success('Quantity modified successful!')
      },
      err => {
        console.log(err)
      }
    )
  }
  openDialog(body){
    this.dialog.open(FinishDailogOrderComponent, {width: '450px',data: body})
  }
  checkOut(){
    this.service.checkOut(this.userName).subscribe(
      (res: any) => {
        this.getBag()
        var order = ''
        for(const bag of this.shoppingBag){
          order = order + bag.name + ' quantity: ' + bag.quantity + ', '
        }
        order = order + 'total: ' + this.total

        var body = {
          UserName: this.userName,
          Order: order,
          Address: '',
          Status: 'Pending',
          PaymentMethod: '',
          Contact: ''
        }
        this.openDialog(body)
      },
      err =>{
        console.log(err)
      }
    )
  }

}
