import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-accident-details-dialog',
  templateUrl: './accident-details-dialog.component.html',
  styleUrls: ['./accident-details-dialog.component.css']
})
export class AccidentDetailsDialogComponent implements OnInit {
  currentAccident: object = {
    agent: "",
    date: "",
    guiltyPeopleAccidentsCommitted: null,
    guiltyPeopleAccidentsInvolved: null,
    guiltyPeopleAge: null,
    guiltyPeopleName: "",
    guiltyPeopleSex: "",
    hour: null,
    id: null,
    innocentPeopleAccidentsCommitted: null,
    innocentPeopleActidentsInvolved: null,
    innocentPeopleAge: null,
    innocentPeopleName: "",
    innocentPeopleSex: "",
    location: "",
    minute: null,
    photo: "",
    settled: false,
    type: "",
    vehicle: ""
  };

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private service: AccidentService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getCurrentAccident()
  }

  getCurrentAccident(){
    this.service.getAccident(this.data).subscribe(
      res => {
        this.currentAccident = res
      }
    )
  }
  settle(idAccident){
    this.service.settle(idAccident).subscribe(
      res => {
        this.toastr.success(`Accident settled successfully`, 'Success!')
        this.getCurrentAccident()
      },
      err => {
        this.toastr.error('Unauthorized action' ,'Error!')
      }
    )
  }

}
