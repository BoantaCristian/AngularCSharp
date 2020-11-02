import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-people-dialog',
  templateUrl: './people-dialog.component.html',
  styleUrls: ['./people-dialog.component.css']
})
export class PeopleDialogComponent implements OnInit {

  person = this.formBuilder.group({
    Name: ['', Validators.required],
    Age: [0, Validators.required],
    Sex: ['', Validators.required],
    BirthDate: ['', Validators.required],
    Address: ['', Validators.required],
    PhoneNumber: ['', Validators.required]
  })

  genders = ['Masculin', 'Feminin']

  constructor(private formBuilder: FormBuilder, private toastr: ToastrService, private service: AccidentService) { }

  ngOnInit() {
  }

  addPeople(){
    var now = new Date()
    
    var birthDate: Date = this.person.value.BirthDate

    if(now.getFullYear() == birthDate.getFullYear()){
      this.toastr.error('Invalid date', 'Failed')
      return;
    }
    if(now.getMonth() >= birthDate.getMonth() && now.getDate() >= birthDate.getDate()){
      this.person.value.Age = now.getFullYear() - birthDate.getFullYear()
      this.person.value.Age++
    }
    else{
      this.person.value.Age = now.getFullYear() - birthDate.getFullYear()
    }
    if(this.person.value.Age <= 1){
      this.toastr.error('Invalid date', 'Failed')
      return
    }
    console.log(this.person.value)
    
    this.service.addPeople(this.person.value).subscribe(
      res => {
        this.toastr.success(`Person ${this.person.value.Name} added successfuly`, 'Succes!')
      }
    )

  }

}
