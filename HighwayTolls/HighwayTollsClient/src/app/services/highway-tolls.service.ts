import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class HighwayTollsService {
  URL = "http://localhost:61509/api"
  constructor(private http: HttpClient) { }

  register(body){
    return this.http.post(`${this.URL}/User/Register`, body)
  }
  registerByAdmin(body){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Admin/Register`, body, {headers: token})
  }
  login(body){
    return this.http.post(`${this.URL}/User/Login`, body)
  }
  getUser(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUser`, {headers: token})
  }
  getUsers(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Admin/Users`, {headers: token})
  }
  getUserRoles(){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Admin/UserRoles`, {headers: token})
  }
  getRoles(){
    return this.http.get(`${this.URL}/Admin/Roles`)
  }
  getLocations(){
    return this.http.get(`${this.URL}/Highway/Locations`)
  }
  getTollBooths(id){
    return this.http.get(`${this.URL}/Highway/TollBooths/${id}`)
  }
  getCategories(){
    return this.http.get(`${this.URL}/Highway/Categories`)
  }
  getPrice(idLoc, idCateg){
    return this.http.get(`${this.URL}/Highway/GetPrice/${idLoc}/${idCateg}`)
  }
  getHistory(){
    return this.http.get(`${this.URL}/Highway/History`)
  }
  confirmPayment(userName, idLoc, body){
    return this.http.post(`${this.URL}/Highway/PostHistory/${userName}/${idLoc}`, body)
  }
  deleteUser(userName){
    var token = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Admin/Delete/${userName}`, {headers: token})
  }
  monthIncome(month){
    return this.http.get(`${this.URL}/Highway/MonthIncome/${month}`)
  }
  calculatePriceRoute(idStartLocation, idFinishLocation, idCategory){
    return this.http.get(`${this.URL}/Highway/CalculateRoute/${idStartLocation}/${idFinishLocation}/${idCategory}`)
  }
}
