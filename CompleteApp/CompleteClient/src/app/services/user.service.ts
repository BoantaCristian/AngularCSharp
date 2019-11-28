import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  URL = 'http://localhost:57805/api'

  constructor(private http: HttpClient) { }
  //manage user
  register(body){
    return this.http.post(`${this.URL}/User/Register` ,body)
  }
  login(body){
    return this.http.post(`${this.URL}/User/Login`, body)
  }
  getUser(){
    var tokenHeader = new HttpHeaders({'Authorization':'Bearer '+localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/GetUser`,{headers: tokenHeader})
  }
  //manage parents
  getParents(){
    return this.http.get(`${this.URL}/Parents`)
  }

  addParent(body){
    return this.http.post(`${this.URL}/Parents`, body)
  }
  //manage children
  getChildren(id){
    return this.http.get(`${this.URL}/Parents/${id}`)
  }

  getAllChildren(){
    return this.http.get(`${this.URL}/Children`)
  }

  addChild(body){
    return this.http.post(`${this.URL}/Children`, body)
  }

  deleteChild(id){
    return this.http.delete(`${this.URL}/Children/${id}`)
  }
}
