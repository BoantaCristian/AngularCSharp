import { Component, OnInit } from '@angular/core';
import { MountainZoneService } from '../services/mountain-zone.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  teams: Object;
  formModel = {
    Name: '',
    Telephone: '',
    Experience: '',
    TeamId: ''
  }
  members: Object;
  constructor(private service: MountainZoneService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getTeams()
    this.getMembers()
  }
  getTeams(){
    this.service.getTeams().subscribe(
      res => {
        this.teams = res
      }
    )
  }
  addMember(){
    this.service.addMember(this.formModel).subscribe(
      (res:any) => {
        this.toastr.success(`Member ${res.name} added in team ${res.team.name}`,'Success')
        this.getMembers()
        this.formModel.Name = null
        this.formModel.Telephone = null
        this.formModel.Experience = null
        this.formModel.TeamId = null
      }
    )
  }
  getMembers(){
    this.service.getAllMembers().subscribe(
      res => {
        this.members = res
      }
    )
  }
  deleteMember(id){
    this.service.deleteMember(id).subscribe(
      res =>{
        this.toastr.success('Success')
        this.getMembers()
      }
    )
  }

}
