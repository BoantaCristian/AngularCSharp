import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AccidentService {
  URI = "http://localhost:53015/api"
  constructor(private http: HttpClient) { }
  
  register(body){
    return this.http.post(`${this.URI}/Accident/Register`, body)
  }
  login(body){
    return this.http.post(`${this.URI}/Accident/Login`, body)
  }
  getUser(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetUser`, {headers: token})
  }
  getUsers(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetUsers`, {headers: token})
  }
  getAdmins(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetAdmins`, {headers: token})
  }
  getSupervisors(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetSupervisors`, {headers: token})
  }
  getAgents(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetAgents`, {headers: token})
  }
  getSupervisorAgents(supervisorName){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetAgentsOfSupervisor/${supervisorName}`, {headers: token})
  }
  getPeople(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetPeople`, {headers: token})
  }
  addPeople(body){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.post(`${this.URI}/Accident/AddPeople`, body, {headers: token})
  }
  getAccidents(){
    return this.http.get(`${this.URI}/Accident/GetAccidents`)
  }
  getSupervisorAgentsAccidents(supervisorName){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetAccidentsOfAgentsOfSupervisor/${supervisorName}`, { headers: token })
  }
  getAgentsAccidents(agentName){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetAccidentsOfAgentsOfAgent/${agentName}`, { headers: token })
  }
  getAccident(idAccident){
    return this.http.get(`${this.URI}/Accident/GetAccident/${idAccident}`)
  }
  getSeverities(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/GetSeverities`, {headers: token})
  }
  deleteUser(userName){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.delete(`${this.URI}/Accident/DeleteUser/${userName}`, {headers: token})
  }
  addAccident(body){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.post(`${this.URI}/Accident/AddAccident`, body, {headers: token})
  }
  deleteAccident(idAccident){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.delete(`${this.URI}/Accident/DeleteAccident/${idAccident}`, {headers: token})
  }
  deletePerson(idPerson){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.delete(`${this.URI}/Accident/DeletePeople/${idPerson}`, {headers: token})
  }
  settle(idAccident){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/Settled/${idAccident}`, {headers: token})
  }
  statistics(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URI}/Accident/Statistics`, {headers: token})
  }
}
