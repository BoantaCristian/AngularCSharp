import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';
import { AccidentDetailsDialogComponent } from '../accident-details-dialog/accident-details-dialog.component';
import { AddAccidentDialogComponent } from '../add-accident-dialog/add-accident-dialog.component';
import { PeopleDialogComponent } from '../people-dialog/people-dialog.component';
import { RegisterDialogComponent } from '../register-dialog/register-dialog.component';
import {TooltipPosition} from '@angular/material/tooltip';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  mismatch: boolean;
  roles = ['Admin', 'Supervizor', 'Agent']
  admins: any = []
  supervisors: object = []
  agents: object = []
  people: object = []
  accidents: any = [];
  statistics: any = [];
  position: TooltipPosition = 'above'
  ascending: any = true;

  constructor( public dialog:MatDialog, private form: FormBuilder, private service: AccidentService, private toastr: ToastrService, private router: Router) { }

  registerForm = this.form.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['', Validators.required]
  })

  ngOnInit() {
    this.getAllUsers()
    this.getAccidents()
    this.getStatistics()
    setTimeout(() => {
      this.sort()
    }, 500);
    
  }

  register(){
    var body = {
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      Password: this.registerForm.value.Password,
      Role: this.registerForm.value.Role
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} registered with success`, "Success!")
          this.registerForm.reset()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username taken', 'Registration failed!')
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
      },
      err =>{
        console.log(err)
      }
    )
  }

  matchPasswords(){
    if(this.registerForm.value.Password == this.registerForm.value.ConfirmPassword)
      this.mismatch = false
    else
      this.mismatch = true
  }

  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    this.router.navigateByUrl('user/login')
  }

  openRegisterDialog(){
    var registerDialog = this.dialog.open(RegisterDialogComponent)
    registerDialog.afterClosed().subscribe( () =>{
      setTimeout(() => {
        this.getAllUsers()
      }, 300);
    }
    )
  }
  openPeopleDialog(){
    var peopleDialog = this.dialog.open(PeopleDialogComponent)
    peopleDialog.afterClosed().subscribe( () =>
      setTimeout(() => {
        this.getAllUsers()
        this.getStatistics()
      }, 300)
    )
  }
  openAccidentDetailsDialog(idAccident){
    this.dialog.open(AccidentDetailsDialogComponent, {data: idAccident})
  }
  openAddAccidentDetailsDialog(){
    var addAccidentDialog = this.dialog.open(AddAccidentDialogComponent, {data: ''})
    addAccidentDialog.afterClosed().subscribe( () => 
    setTimeout(() => {
          this.getAccidents()
          this.getStatistics()
          setTimeout(() => {
            this.sort()
          }, 200);
    }, 400))
  }
  openUpdateAccidentDialog(idAccident){
    console.log(idAccident)
    var updateAccidentDialog = this.dialog.open(AddAccidentDialogComponent, {data: idAccident})
    updateAccidentDialog.afterClosed().subscribe( () => 
    setTimeout(() => {
          this.getAccidents()
          this.getStatistics()
          setTimeout(() => {
            this.sort()
          }, 200);
    }, 400))
  }
  getAllUsers(){
    this.service.getAdmins().subscribe(
      res => {
        this.admins = res
      }
    )
    this.service.getSupervisors().subscribe(
      res => {
        this.supervisors = res
      }
    )
    this.service.getAgents().subscribe(
      res => {
        this.agents = res
      }
    )
    this.service.getPeople().subscribe(
      res => {
        this.people = res
      }
    )
  }
  getAccidents(){
    this.service.getAccidents().subscribe(
      res => {
        this.accidents = res
      }
    )
  }
  deleteUser(userName){
    this.service.deleteUser(userName).subscribe(
      res => {
        this.toastr.success(`User ${userName} deleted successfully`, 'Success!')
        this.getAllUsers()
      },
      err => {
        console.log(err)
        this.toastr.error(`${err.error.message}`,'Failed!')
      }
    )
  }
  deleteAccident(idAccident){
    this.service.deleteAccident(idAccident).subscribe(
      res => {
        this.toastr.success('Accident deleted successfully', 'Success!')
        this.getAccidents()
        this.getStatistics()
      }
    )
  }
  deletePerson(idPerson){
    this.service.deletePerson(idPerson).subscribe(
      res => {
        this.toastr.success('Person deleted successfully', 'Success!')
        this.getAllUsers()
        this.getStatistics()
      },
      err => {
        this.toastr.error(`Person is involved in an accident`,'Error!')
      }
    )
  }
  getStatistics(){
    this.service.statistics().subscribe(
      res => {
        this.statistics = res
      }
    )
  }
  compareHoursAscending( a, b ) {
    if ( a.hour < b.hour ){
      return -1;
    }
    if ( a.hour > b.hour ){
      return 1;
    }
    return 0;
  }
  compareHoursDescending( a, b ) {
    if ( a.hour > b.hour ){
      return -1;
    }
    if ( a.hour < b.hour ){
      return 1;
    }
    return 0;
  }
  compareAccidentsAscending( a, b ) {
    if ( a.accidents < b.accidents ){
      return -1;
    }
    if ( a.accidents > b.accidents ){
      return 1;
    }
    return 0;
  }
  compareAccidentsDescending( a, b ) {
    if ( a.accidents > b.accidents ){
      return -1;
    }
    if ( a.accidents < b.accidents ){
      return 1;
    }
    return 0;
  }
  comparePercentageAscending( a, b ) {
    if ( a.percentage < b.percentage ){
      return -1;
    }
    if ( a.percentage > b.percentage ){
      return 1;
    }
    return 0;
  }
  comparePercentageDescending( a, b ) {
    if ( a.percentage > b.percentage ){
      return -1;
    }
    if ( a.percentage < b.percentage ){
      return 1;
    }
    return 0;
  }
  compareLocationAscending( a, b ) {
    if ( a.location < b.location ){
      return -1;
    }
    if ( a.location > b.location ){
      return 1;
    }
    return 0;
  }
  compareLocationDescending( a, b ) {
    if ( a.location > b.location ){
      return -1;
    }
    if ( a.location < b.location ){
      return 1;
    }
    return 0;
  }
  sort(){
    this.statistics.timeRiskList.sort( this.compareHoursAscending )
    this.statistics.locationsRiskList.sort( this.compareLocationAscending )
  }
  sortByProperty(property){
    switch(property){
      case 'hour':{
        if(this.ascending){
          this.statistics.timeRiskList.sort( this.compareHoursDescending )
          this.ascending = false

        }
        else{
          this.statistics.timeRiskList.sort( this.compareHoursAscending )
          this.ascending = true
        }
        break;
      }
      case 'accidents':{
        if(this.ascending){
          this.statistics.timeRiskList.sort( this.compareAccidentsAscending )
          this.ascending = false

        }
        else{
          this.statistics.timeRiskList.sort( this.compareAccidentsDescending )
          this.ascending = true
        }
        break;
      }
      case 'percentage':{
        if(this.ascending){
          this.statistics.timeRiskList.sort( this.comparePercentageAscending )
          this.ascending = false

        }
        else{
          this.statistics.timeRiskList.sort( this.comparePercentageDescending )
          this.ascending = true
        }
        break;
      }
      case 'location':{
        if(this.ascending){
          this.statistics.locationsRiskList.sort( this.compareLocationDescending )
          this.ascending = false

        }
        else{
          this.statistics.locationsRiskList.sort( this.compareLocationAscending )
          this.ascending = true
        }
        break;
      }
      case 'locationAccidents':{
        if(this.ascending){
          this.statistics.locationsRiskList.sort( this.compareAccidentsAscending )
          this.ascending = false

        }
        else{
          this.statistics.locationsRiskList.sort( this.compareAccidentsDescending )
          this.ascending = true
        }
        break;
      }
      case 'locationPercentage':{
        if(this.ascending){
          this.statistics.locationsRiskList.sort( this.comparePercentageAscending )
          this.ascending = false

        }
        else{
          this.statistics.locationsRiskList.sort( this.comparePercentageDescending )
          this.ascending = true
        }
        break;
      }
    }
  }
}
