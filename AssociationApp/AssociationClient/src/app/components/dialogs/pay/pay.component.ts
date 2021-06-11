import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-pay',
  templateUrl: './pay.component.html',
  styleUrls: ['./pay.component.css']
})
export class PayComponent implements OnInit {
  maxValueField: any;
  fileToUpload: File;
  imagePath: any;
  imageLink: any;
  

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private service: AssociationService, private toastr: ToastrService) { }

  payForm = this.formBuilder.group({
    PaymentId: [this.data.actions.id, Validators.required],
    ClientUserName: [this.data.actions.userName, Validators.required],
    AmountPaid: [(Math.round(this.data.actions.totalDueWithPenalties * 100) / 100).toFixed(2), Validators.required],
    WorkingCapital: [this.data.actions.workingCapitalStatus],
    Sanitation: [this.data.actions.sanitationStatus],
    ReceiptPaper: ''
  })

  ngOnInit() {
    console.log(this.data.actions.totalDueWithPenalties)
  }

  pay(){
    this.service.pay(this.payForm.value).subscribe(
      res => {
        this.toastr.success('Paid successfully!', 'Success!')
        this.service.updatePayments().subscribe(
          res => {
            this.toastr.success('Payments and penalties updated!', 'Success!')
          }
        )
      }
    )
  }

  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0)

    var reader = new FileReader()
    reader.onload = (event:any) => {
      this.imagePath = event.target.result
    }
    reader.readAsDataURL(this.fileToUpload)
    this.payForm.value.ReceiptPaper = "../../../assets/" + this.fileToUpload.name
    this.imageLink = this.payForm.value.ReceiptPaper
  }

}
