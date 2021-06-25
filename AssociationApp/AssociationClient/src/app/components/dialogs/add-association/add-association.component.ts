import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AssociationService } from 'src/app/services/association.service';

@Component({
  selector: 'app-add-association',
  templateUrl: './add-association.component.html',
  styleUrls: ['./add-association.component.css']
})
export class AddAssociationComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private service: AssociationService, private toastr: ToastrService) { }

  associationDetailsForm = this.formBuilder.group({
    Description: ['', Validators.required],
    Location: ['', Validators.required],
    Program: ['', Validators.required]
  })
  associationMaintenanceForm = this.formBuilder.group({
    WorkingCapital: [null, Validators.required],
    Sanitation: [null, Validators.required],
    DayPenalty: [null, Validators.required],
  })
  isLinear:boolean = true
  
  ngOnInit() {
  }

  addAssociation(){
    var associationForm = {
      ...this.associationDetailsForm.value,
      ...this.associationMaintenanceForm.value
    }
    this.service.addAssociation(associationForm).subscribe(
      res => { },
      err => {
        this.toastr.error('Error!')
        console.log(err)
      }
    )
  }

}
