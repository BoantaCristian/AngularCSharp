import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class PhoneShopService {

  constructor(private http: HttpClient) { }

  URL = "http://localhost:50680/api"
  register(body){
    return this.http.post(`${this.URL}/User/Register`, body)
  }
  login(body){
    return this.http.post(`${this.URL}/User/Login`, body)
  }
  getUser(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUser`, {headers : token})
  }
  getPhones(){
    return this.http.get(`${this.URL}/Phone/GetPhones`)
  }
  getPhonesWithCompanies(){
    return this.http.get(`${this.URL}/Phone/GetPhonesWithCompanies`)
  }
  getPhoneDetails(idPhone){
    return this.http.get(`${this.URL}/Phone/GetPhoneDetails/${idPhone}`)
  }
  getAllPhonesDetails(){
    return this.http.get(`${this.URL}/Phone/GetAllPhonesDetails`)
  }
  getCompanies(){
    return this.http.get(`${this.URL}/Phone/GetCompanies`)
  }
  checkBag(userName){
    return this.http.get(`${this.URL}/Phone/CheckBag/${userName}`)
  }
  getBag(userName){
    return this.http.get(`${this.URL}/Phone/GetShoppingBag/${userName}`)
  }
  deletePhoneFromBag(idPhone){
    return this.http.get(`${this.URL}/Phone/DeletePhoneBag/${idPhone}`)
  }
  editQuantity(idPhone, userName, body){
    return this.http.put(`${this.URL}/Phone/EditQuantity/${idPhone}/${userName}`, body)
  }
  addToBag(userId, phoneId, body){
    return this.http.post(`${this.URL}/Phone/AddToCart/${userId}/${phoneId}`, body)
  }
  checkOut(userName){
    return this.http.delete(`${this.URL}/Phone/CheckOut/${userName}`)
  }
  addToHistoric(body){
    return this.http.post(`${this.URL}/Phone/AddToHistoric`, body)
  }
  getUsers(){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUsers`, {headers: token})
  }
  deleteUser(userName){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeleteUser/${userName}`, {headers: token})
  }
  addPhone(body){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.post(`${this.URL}/User/AddPhone`, body, {headers: token})
  }
  addCompany(body){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.post(`${this.URL}/User/AddCompany`, body, {headers: token})
  }
  addPhoneDetails(body){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.post(`${this.URL}/User/AddPhoneDetails`, body, {headers: token})
  }
  deletePhone(idPhone){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeletePhone/${idPhone}`, {headers: token})
  }
  updatePhone(idPhone, body){
    return this.http.put(`${this.URL}/Phone/UpdatePhone/${idPhone}`, body)
  }
  getHistoric(){
    return this.http.get(`${this.URL}/Phone/GetHistoric`)
  }
  deliver(idHistoric){
    var token = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/Deliver/${idHistoric}`, {headers: token})
  }
}
