import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
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

  constructor(private formBuilder: FormBuilder, private toastr: ToastrService, private service: AssociationService) { }

  paymentDetailsForm = this.formBuilder.group({
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
        console.log(res.id)
      }
    )
  }

  checkDate(){
    this.incorrectDate = false
    var currentDate = new Date()
    if(currentDate < this.paymentDetailsForm.value.Date){
      this.incorrectDate = true
      this.toastr.error(`Cannot pay for the future!`)
      this.paymentDetailsForm.value.Date = ''
    }
  }

  emitPayment(){
    this.paymentDetailsForm.value.UtilitiesPaper = this.imageLink
    var payment = {
      ClientId: this.userDetails.id,
      ...this.paymentDetailsForm.value,
      ...this.utilitiesValuesForm.value
    }
    console.log(payment)
    this.service.emitPayment(payment).subscribe(
      (res: any) => {
        console.log(res)
          this.toastr.success('Payment added successfully', 'Success!')
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`, 'Error!')
      }
    )
  }

}
