import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-emit-payment',
  templateUrl: './emit-payment.component.html',
  styleUrls: ['./emit-payment.component.css']
})
export class EmitPaymentComponent implements OnInit {
  fileToUpload: File;
  isLinear:boolean = true
  imagePath: string = "../../../assets/default-image.png"
  imageLink: any;
  userDetails: any = {};
  incorrectDate: boolean;
  clients: any;
  userIdSelected: boolean = false;
  clientsOfRepresentative: any;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private toastr: ToastrService, private service: AssociationService) { }

  paymentDetailsForm = this.formBuilder.group({
    ClientId: '',
    Date: ['', Validators.required],
    UtilitiesPaper: ['', Validators.required]
  })
  utilitiesValuesForm = this.formBuilder.group({
    HotWaterKitchenQuantity: [null, Validators.required],
    ColdWaterKitchenQuantity: [null, Validators.required],
    HotWaterBathroomQuantity: [null, Validators.required],
    ColdWaterBathroomQuantity: [null, Validators.required],
    GasQuantity: [null, Validators.required],
    ElectricityQuantity: [null, Validators.required]
  })

  ngOnInit() {
    this.getUser()
  }

  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0)

    var reader = new FileReader()
    reader.onload = (event:any) => {
      this.imagePath = event.target.result
    }
    reader.readAsDataURL(this.fileToUpload)
    this.paymentDetailsForm.value.UtilitiesPaper = "../../../assets/" + this.fileToUpload.name
    this.imageLink = this.paymentDetailsForm.value.UtilitiesPaper
  }

  getUser(){
    this.service.getUser().subscribe(
      (res: any) => {
        this.userDetails = res
        this.paymentDetailsForm.setValue({
          ClientId: res.id,
          Date: '',
          UtilitiesPaper: ''
        })
        this.getClients()
      }
    )
  }

  getClients(){
    if(this.data == 'Admin'){
      this.service.getUsers().subscribe(
        (res: any) => {
          this.clients = res.clients
        }
      )
    }
    if(this.data == 'Representative'){
      this.service.getClientsOfRepresentative(this.userDetails.id).subscribe(
        (res: any) => {
          this.clients = res
        }
      )
    }
  }

  checkDate(){
    this.incorrectDate = false
    var currentDate = new Date()
    if(currentDate < this.paymentDetailsForm.value.Date){
      this.incorrectDate = true
      this.toastr.warning(`Cannot pay for the future!`)
      this.paymentDetailsForm.value.Date = ''
    }
  }

  selectClient(){
    this.userIdSelected = true
  }

  emitPayment(){
    this.paymentDetailsForm.value.UtilitiesPaper = this.imageLink
    if(this.data == 'Client'){
      var payment = {
        ...this.paymentDetailsForm.value,
        ...this.utilitiesValuesForm.value
      }
    }
    else{
      var payment = {
        ...this.paymentDetailsForm.value,
        ...this.utilitiesValuesForm.value
      }
    }
    this.service.emitPayment(payment).subscribe(
      (res: any) => {
          this.toastr.success('Payment added successfully', 'Success!')
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`, 'Error!')
      }
    )
  }

}
