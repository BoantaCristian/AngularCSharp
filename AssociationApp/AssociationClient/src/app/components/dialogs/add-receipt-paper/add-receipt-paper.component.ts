import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-add-receipt-paper',
  templateUrl: './add-receipt-paper.component.html',
  styleUrls: ['./add-receipt-paper.component.css']
})
export class AddReceiptPaperComponent implements OnInit {
  fileToUpload: File;
  imagePath: string = "../../../assets/default-image.png"
  imageLink: any;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private service: AssociationService, private toastr: ToastrService) { }

  ngOnInit() {
  }

  receiptPaperForm = this.formBuilder.group({
    ReceiptPaper: ['', Validators.required]
  })

  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0)

    var reader = new FileReader()
    reader.onload = (event:any) => {
      this.imagePath = event.target.result
    }
    reader.readAsDataURL(this.fileToUpload)
    this.receiptPaperForm.value.ReceiptPaper = "../../../assets/" + this.fileToUpload.name
    this.imageLink = this.receiptPaperForm.value.UtilitiesPaper
  }

  addPaper(){
    this.service.addReceiptPaper(this.data.id, this.receiptPaperForm.value).subscribe(
      res => {
        this.toastr.success('Receipt paper added successfully!', 'Success!')
      }
    )
  }

}
