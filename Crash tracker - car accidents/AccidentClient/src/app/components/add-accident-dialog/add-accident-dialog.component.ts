import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDatepicker, MatDatepickerInputEvent, MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-add-accident-dialog',
  templateUrl: './add-accident-dialog.component.html',
  styleUrls: ['./add-accident-dialog.component.css']
})
export class AddAccidentDialogComponent implements OnInit {

  isLinear = true
  photoLink = ''
  hours = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18, 19,20,21,22,23]
  severities: any;
  fileToUpload: File;
  imagePath: string = "../../../assets/default-image.png"
  agents: Object;
  people: Object;
  accident: any = {
    Date: '',
    Hour: null,
    Minute: null,
    Location: '',
    Photo: "",
    Guilty: null,
    Innocent: null,
    AgentName: '',
    Severity: null
  }
  identicPeople: boolean = false;
  imageLink: any;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private service: AccidentService, private toastr: ToastrService) { }

  accidentDetailsForm = this.formBuilder.group({
    Date: ['', Validators.required],
    Hour: [null, Validators.required],
    Minute: [null, Validators.required],
    Location: ['', Validators.required]
  })
  involvedPeopleForm = this.formBuilder.group({
    Guilty: [null, Validators.required],
    Innocent: [null, Validators.required],
    AgentName: ['']
  })
  photoForm = this.formBuilder.group({
    Photo: "",
    Severity: [null, Validators.required]
  })

  ngOnInit() {
    this.getSeverities()
    this.getPeople()
    this.getAgents()
  }

  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0)

    var reader = new FileReader()
    reader.onload = (event:any) => {
      this.imagePath = event.target.result
    }
    reader.readAsDataURL(this.fileToUpload)
    this.photoForm.value.Photo = "../../../assets/" + this.fileToUpload.name
    this.imageLink = this.photoForm.value.Photo
  }
  getSeverities(){
    this.service.getSeverities().subscribe(
      res => {
        this.severities = res
      }
    )
  }
  getAgents(){
    if(localStorage.getItem('role') == 'Admin'){
      this.service.getAgents().subscribe(
        res => {
          this.agents = res
        }
      )
    }
    else if(localStorage.getItem('role') == 'Supervizor'){
      this.service.getSupervisorAgents(this.data.userName).subscribe(
        res =>{
          this.agents = res
        }
      )
    }
        
  }
  getPeople(){
    this.service.getPeople().subscribe(
      res => {
        this.people = res
      }
    )
  }
  updatePeople(){
    this.accident.Guilty = this.involvedPeopleForm.value.Guilty
    this.accident.Innocent = this.involvedPeopleForm.value.Innocent
    if(this.involvedPeopleForm.value.Guilty == this.involvedPeopleForm.value.Innocent)
      this.identicPeople = true
    else
      this.identicPeople = false
  }

  checkDate(){
    var currentDate = new Date()
    if(currentDate < this.accidentDetailsForm.value.Date){
      this.toastr.error('Accident should happen before the current date!', 'Invalid')
      this.accidentDetailsForm.reset({ Date: '',
                                       Hour: this.accidentDetailsForm.value.Hour,
                                       Minute: this.accidentDetailsForm.value.Minute,
                                       Location: this.accidentDetailsForm.value.Location,})
    }   
    if(new Date(new Date().setHours(0, 0, 0, 0)) <= this.accidentDetailsForm.value.Date && currentDate.getHours() < this.accidentDetailsForm.value.Hour){
      this.toastr.error('Hour of the accident cant be in the future!', 'Invalid')
      this.accidentDetailsForm.reset({ Date: this.accidentDetailsForm.value.Date,
                                       Hour: null,
                                       Minute: this.accidentDetailsForm.value.Minute,
                                       Location: this.accidentDetailsForm.value.Location,})
    }
    if(new Date(new Date().setHours(0, 0, 0, 0)) <= this.accidentDetailsForm.value.Date && currentDate.getHours() == this.accidentDetailsForm.value.Hour && currentDate.getMinutes() < this.accidentDetailsForm.value.Minute){
      this.toastr.error('Minute of the accident cant be in the future!', 'Invalid')
      this.accidentDetailsForm.reset({ Date: this.accidentDetailsForm.value.Date,
                                       Hour: this.accidentDetailsForm.value.Hour,
                                       Minute: null,
                                       Location: this.accidentDetailsForm.value.Location,})
    }
  }

  addAccident(){
    var accident = {
      Date: this.accidentDetailsForm.value.Date,
      Hour: this.accidentDetailsForm.value.Hour,
      Minute: this.accidentDetailsForm.value.Minute,
      Location: this.accidentDetailsForm.value.Location,
      Photo: this.imageLink,
      Severity: this.photoForm.value.Severity,
      Guilty: this.involvedPeopleForm.value.Guilty,
      Innocent: this.involvedPeopleForm.value.Innocent,
      AgentName: this.involvedPeopleForm.value.AgentName
    }
    if(this.data.role == "Agent"){
      accident.AgentName = this.data.userName
    }
    this.service.addAccident(accident).subscribe(
      res => {
        this.toastr.success('Accident added successfully', 'Success!')
      }
    )
  }

}
