import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  URL = "http://localhost:53825/api"

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
  getParents(){
    return this.http.get(`${this.URL}/Parents`)
  }
  getChildrenOfParent(id){
    return this.http.get(`${this.URL}/Parents/${id}`)
  }
  getUncles(){
    return this.http.get(`${this.URL}/Uncles/GetUncles`)
  }
  getFullUncles(){
    return this.http.get(`${this.URL}/Uncles/GetFullUncles`)
  }
  getValues(){
    return this.http.get("http://localhost:53825/api/values")
  }
}
