import { Component, OnInit } from '@angular/core';
import { MountainZoneService } from 'src/app/services/mountain-zone.service';
import { Router } from '@angular/router';
import { ConcatSource } from 'webpack-sources';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userDetails: Object = {
    userName: '',
    email: '',
    role: ''
  };
  logged: boolean;
  admin: boolean;
  zones: Object;
  routes: Object;
  selectedZone;
  accommodations: Object;
  viewDetail = true;
  selectZone = false;
  accommodationPoint: Object;
  routeObjectives: Object;
  selectedRoute: boolean;
  selectedTeam
  teams: Object;
  members: Object;
  teamActivityZones: any;
  membersPerZone: Object;

  constructor(private service: MountainZoneService, private router: Router) { }

  ngOnInit() {
    //localStorage.removeItem('token')
    this.getUser()
    this.getZones()
    this.getAccommodations()
    this.getTeams()
    this.getMembersPerZone()
  }

  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.userDetails = res
          this.logged = true
          localStorage.setItem('role', res.role)
          if(localStorage.getItem('role') == 'Admin'){
            this.admin = true
          }
          else
            this.admin = false
        }
      )
    }
    else
      this.logged = false
  }

  logout(){
    this.router.navigateByUrl('/user/login')
    localStorage.removeItem('token')
    localStorage.removeItem('role')
  }

  getZones(){
    this.service.getZones().subscribe(
      res => {
        this.zones = res
      }
    )
  }
  getAccommodations(){
    this.service.getAccommodations().subscribe(
      res => {
        this.accommodations = res
      }
    )
  }
  viewDetails(){
    if(this.viewDetail){
      this.viewDetail = false
    } else
    if(!this.viewDetail){
      this.viewDetail = true
    }
  }
  confirmZone(){
    this.selectZone = true
    this.service.getAccommodationPoint(this.selectedZone).subscribe(
      (res: any) => {
        this.accommodationPoint = res.accommodations
      }
    )
    this.service.getRoute(this.selectedZone).subscribe(
      (res: any) => {
        this.routes = res.routes
      }
    )
  }
  cancelZone(){
    this.selectZone = false
    this.selectedZone = null
  }
  getObjectiveOnRoute(idRoute){
    this.selectedRoute = true
    this.service.getObjectiveOnRoute(idRoute).subscribe(
      res =>{
        this.routeObjectives = res
      }
    )
  }
  getTeams(){
    this.service.getTeams().subscribe(
      res => {
        this.teams = res
      }
    )
  }
  selectTeam(idTeam){
    this.selectedTeam = true
    this.service.getMembers(idTeam).subscribe(
      (res: any) => {
        this.members = res.members
      }
    )
    this.service.getTeamActivity(idTeam).subscribe(
      (res:any) => {
        this.teamActivityZones = res
        console.log(res)
      }
    )
  }
  cancelTeam(){
    this.selectedTeam = false
    this.members = null
  }
  getMembersPerZone(){
    this.service.getMembersPerZone().subscribe(
      res => {
        console.log(res)
        this.membersPerZone = res
      }
    )
  }
}
