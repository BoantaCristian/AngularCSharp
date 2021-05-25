import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-add-provider',
  templateUrl: './add-provider.component.html',
  styleUrls: ['./add-provider.component.css']
})
export class AddProviderComponent implements OnInit {
  
  constructor(public dialogRef: MatDialogRef<AddProviderComponent>, private formBuilder: FormBuilder, private service: AssociationService, private toastr: ToastrService) { }
  
  isLinear:boolean = true
  addedSuccessful: boolean = true

  providerDetailsForm = this.formBuilder.group({
    Name: ['', Validators.required],
    Location: ['', Validators.required],
    Program: ['', Validators.required]
  })
  providerPricesForm = this.formBuilder.group({
    HotWaterLiterPrice: [null, Validators.required],
    ColdWaterLiterPrice: [null, Validators.required],
    GasPrice: [null, Validators.required],
    ElectricityPrice: [null, Validators.required],
  })
  
  ngOnInit() {
  }

  addProvider(){
    var providerForm = {
      ...this.providerDetailsForm.value,
      ...this.providerPricesForm.value
    }
    console.log(providerForm)
    this.service.addProvider(providerForm).subscribe(
      res => { },
      err => {
        this.toastr.error('Error!')
        console.log(err)
      }
    )
  }

}
