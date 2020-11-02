import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from "@angular/material";
import { PhoneShopService } from 'src/app/services/phone-shop.service';

@Component({
  selector: 'app-detail-dialog',
  templateUrl: './detail-dialog.component.html',
  styleUrls: ['./detail-dialog.component.css']
})
export class DetailDialogComponent implements OnInit {
  currentPhoneDetails: object ={
    id:'', dimensions: '', weight: '', displayType: '', resolution: '', os: '', mainCamera: '',
    selfieCamera: '', battery: '',
    telephone: {
      name:'', imagePath:''
    }
  };

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private service: PhoneShopService) { }

  ngOnInit() {
    this.getPhoneDetails()
  }
  getPhoneDetails(){
    this.service.getPhoneDetails(this.data).subscribe(
      res => {
        this.currentPhoneDetails = res
      }
    )
  }
  

}
