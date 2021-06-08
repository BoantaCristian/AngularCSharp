import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AssociationService {

  URL = 'http://localhost:53598/api'
  //token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')}) //token generation error: shows unauthorized

  constructor(private http: HttpClient) { }

  //user
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
  getUsers(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUsers`, {headers: token})
  }
  deleteUser(userName){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeleteUser/${userName}`, {headers: token})
  }
  //providers
  getProviders(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/GetProviders`, {headers: token})
  }
  addProvider(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Association/AddProvider`, body, {headers: token})
  }
  deleteProvider(idProvider){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Association/DeleteProvider/${idProvider}`, {headers: token})
  }
  //association
  getAssociations(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/GetAssociations`, {headers: token})
  }
  addAssociation(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Association/AddAssociation`, body, {headers: token})
  }
  deleteAssociation(idAssociation){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Association/DeleteAssociation/${idAssociation}`, {headers: token})
  }
  //payments
  getPayment(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/GetPayments`, {headers: token})
  }
  emitPayment(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Association/EmitPayment`, body, {headers: token})
  }
  updatePayments(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/UpdatePenalties`, {headers: token})
  }
  updatePayment(idPayment){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/UpdatePenalty/${idPayment}`, {headers: token})
  }
  deletePayment(idPayment){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Association/DeletePayment/${idPayment}`, {headers: token})
  }
  pay(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Association/Pay`, body, {headers: token})
  }
  //archives
  getArchive(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/GetArchives`, {headers: token})
  }
  deleteArchive(idArchive){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Association/DeleteArchive/${idArchive}`, {headers: token})
  }
  //receipts
  getReceipts(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/Association/GetReceipts`, {headers: token})
  }
  deleteReceipt(idReceipt){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/Association/DeleteReceipt/${idReceipt}`, {headers: token})
  }
  
}
