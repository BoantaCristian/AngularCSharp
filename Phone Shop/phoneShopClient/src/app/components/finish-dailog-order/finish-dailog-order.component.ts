import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from "@angular/material";
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-finish-dailog-order',
  templateUrl: './finish-dailog-order.component.html',
  styleUrls: ['./finish-dailog-order.component.css']
})
export class FinishDailogOrderComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private router: Router, private service: PhoneShopService, private tosatr: ToastrService) { }

  paymentMethod = ['Cash', 'Card']

  ngOnInit() {
    console.log(this.data)
  }

  placeOrder(){
    this.service.addToHistoric(this.data).subscribe(
      res => {
        this.tosatr.success('Your order has been placed!')
        this.router.navigateByUrl('')
      }
    )
  }

}
