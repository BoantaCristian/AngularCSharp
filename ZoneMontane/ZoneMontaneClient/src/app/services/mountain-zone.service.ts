import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MountainZoneService {

  URL = "http://localhost:49413/api"

  constructor(private http: HttpClient) { }

  register(body){
    return this.http.post(`${this.URL}/User/Register`, body)
  }
  login(body){
    return this.http.post(`${this.URL}/User/Login`, body)
  }
  getUser(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUser`, {headers: token})
  }
  getZones(){
    return this.http.get(`${this.URL}/ZoneMontane/GetZones`)
  }
  getAccommodations(){
    return this.http.get(`${this.URL}/ZoneMontane/GetAllAccommodation`)
  }
  getAccommodationPoint(id){
    return this.http.get(`${this.URL}/ZoneMontane/GetAccommodationPoint/${id}`)
  }
  getTeams(){
    return this.http.get(`${this.URL}/ZoneMontane/GetTeams`)
  }
  getAllMembers(){
    return this.http.get(`${this.URL}/ZoneMontane/GetMembers`)
  }
  getMembers(idTeam){
    return this.http.get(`${this.URL}/ZoneMontane/GetTeamMembers/${idTeam}`)
  }
  getTeamActivity(idTeam){
    return this.http.get(`${this.URL}/ZoneMontane/GetTeamActivity/${idTeam}`)
  }
  getRoute(idZone){
    return this.http.get(`${this.URL}/ZoneMontane/GetRoute/${idZone}`)
  }
  getObjectiveOnRoute(idRoute){
    return this.http.get(`${this.URL}/ZoneMontane/GetObjectives/${idRoute}`)
  }
  getMembersPerZone(){
    return this.http.get(`${this.URL}/ZoneMontane/GetMembersPerZone`)
  }
  addMember(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/ZoneMontane/AddMember`, body, {headers: token})
  }
  deleteMember(id){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
     return this.http.delete(`${this.URL}/ZoneMontane/DeleteMember/${id}` , {headers: token})
  }
}
