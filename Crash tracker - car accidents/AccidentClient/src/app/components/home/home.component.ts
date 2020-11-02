import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from "../../services/accident.service";
import { AccidentDetailsDialogComponent } from '../accident-details-dialog/accident-details-dialog.component';
import { AddAccidentDialogComponent } from '../add-accident-dialog/add-accident-dialog.component';
import { AddAgentDialogComponent } from '../add-agent-dialog/add-agent-dialog.component';
import { PeopleDialogComponent } from '../people-dialog/people-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  logged: boolean;
  currentUserDetails: any = {
    userName: '',
    email: '',
    role: '',
    supervisor: ''
  };
  loggedUserRole: any;
  currnetAgents: Object;
  currentSupervisorAgentsAccidents: any = [];
  people: Object;
  currentAgentsAccidents: any = [];

  constructor( public dialog:MatDialog, private service: AccidentService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.checkLogged()
    this.getUser()
    this.getPeople()
    setTimeout(() => {
      this.getAgents()
    }, 300);
    setTimeout(() => {
      this.getSupervisorAgentsAccidents(this.currentUserDetails.userName)
    }, 300);
    setTimeout(() => {
      this.getAgentsAccidents(this.currentUserDetails.userName)
    }, 300);
  }

  logout(){
    localStorage.removeItem('token')
    this.router.navigateByUrl('/user/login')
  }

  checkLogged(){
    if(localStorage.getItem('token') != null)
      this.logged = true
    else
      this.logged = false
      setTimeout(() => {
        if(localStorage.getItem('role') == 'Admin')
          this.router.navigateByUrl('admin')
      }, 200);
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.currentUserDetails = res
          this.logged = true
          localStorage.setItem('role', res.role)
          this.loggedUserRole = res.role
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
          this.router.navigateByUrl('user/login')
        }
      )
    }
    else
      this.logged = false
  }
  getPeople(){
    this.service.getPeople().subscribe(
      res => {
        this.people = res
      }
    )
  }
  getAgents(){
    if(this.loggedUserRole == 'Supervizor'){
      this.service.getSupervisorAgents(this.currentUserDetails.userName).subscribe(
        res => {
          this.currnetAgents = res
        }
      )
    }
  }
  deleteAgent(userName){
    this.service.deleteUser(userName).subscribe(
      res => {
        this.toastr.success(`User ${userName} deleted successfully`, 'Success!')
        this.getAgents()
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
        this.getSupervisorAgentsAccidents(this.currentUserDetails.userName)
      }
    )
  }
  getSupervisorAgentsAccidents(supervisorName){
    if(this.loggedUserRole == 'Supervizor'){
      this.service.getSupervisorAgentsAccidents(supervisorName).subscribe(
        res => {
          this.currentSupervisorAgentsAccidents = res
        }
      )
    }
  }

  getAgentsAccidents(agentName){
    if(this.loggedUserRole == 'Agent'){
      this.service.getAgentsAccidents(agentName).subscribe(
        res => {
          this.currentAgentsAccidents = res
        }
      )
    }
  }

  openAccidentDetailsDialog(supervisorName){
    this.dialog.open(AccidentDetailsDialogComponent, {data: supervisorName})
  }

  openAddAgentDialog(){
    var addAgentDialog = this.dialog.open(AddAgentDialogComponent, {data: this.currentUserDetails.userName})
    addAgentDialog.afterClosed().subscribe( () => {
      setTimeout(() => {
        this.getAgents()
        }, 300);
      }
    )
  }

  openAddAccidentDetailsDialog(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      userName: this.currentUserDetails.userName,
      role: this.loggedUserRole
    };
    var addAccidentDialog = this.dialog.open(AddAccidentDialogComponent, dialogConfig)
    addAccidentDialog.afterClosed().subscribe( () => 
    setTimeout(() => {
          this.getSupervisorAgentsAccidents(this.currentUserDetails.userName)
          this.getAgentsAccidents(this.currentUserDetails.userName)
    }, 300))
  }

  openPeopleDialog(){
    var peopleDialog = this.dialog.open(PeopleDialogComponent)
    peopleDialog.afterClosed().subscribe( () =>
      setTimeout(() => {
        this.getPeople()
      }, 300)
    )
  }
}