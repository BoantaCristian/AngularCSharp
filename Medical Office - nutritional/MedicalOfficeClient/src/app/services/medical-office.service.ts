import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class MedicalOfficeService {
  URL = 'http://localhost:51775/api'
  constructor(private http: HttpClient) { }
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
  getDoctors(){
    return this.http.get(`${this.URL}/MedicalOffice/GetDoctors`)
  }
  getPatients(){
    return this.http.get(`${this.URL}/MedicalOffice/GetPatients`)
  }
  getCurrentPatients(idDoctor){
    return this.http.get(`${this.URL}/MedicalOffice/GetCurrentPatients/${idDoctor}`)
  }
  getUsers(){
    return this.http.get(`${this.URL}/User/GetUsers`)
  }
  VerifyUser(id){
    var token = new HttpHeaders({'Authorization': 'Bearer '+localStorage.getItem('token')})
    return this.http.get(`${this.URL}/User/VerifyUser/${id}`, {headers : token})
  }
  deleteUser(userId){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeleteUser/${userId}`, {headers: token})
  }
  getAppointments(){
    return this.http.get(`${this.URL}/MedicalOffice/GetAppointments`)
  }
  getHistoric(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/MedicalOffice/GetHistoric`, {headers: token})
  }
  getCurrentAppointments(idDoctor){
    return this.http.get(`${this.URL}/MedicalOffice/GetCurrentAppointments/${idDoctor}`)
  }
  getPatientAppointments(idPatient){
    return this.http.get(`${this.URL}/MedicalOffice/GetPatientAppointments/${idPatient}`)
  }
  addAppointment(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/MedicalOffice/AddAppointment`, body, {headers: token})
  }
  deleteAppointment(appointmentId){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/MedicalOffice/DeleteAppointment/${appointmentId}`, {headers: token})
  }
  getMedicaments(){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.get(`${this.URL}/MedicalOffice/Medicaments`,  {headers: token})
  }
  getIllnesses(){
    return this.http.get(`${this.URL}/MedicalOffice/Illnesses`)
  }
  getPatientIllnesses(idDoctor){
    return this.http.get(`${this.URL}/MedicalOffice/GetPatientIllMedicine/${idDoctor}`)
  }
  addMedicament(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/MedicalOffice/AddMedicament`, body, {headers: token})
  }
  addIllness(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/MedicalOffice/AddIllness`, body, {headers: token})
  }
  addIllnessToExistingPatient(idIllness, idPatient){
    return this.http.get(`${this.URL}/MedicalOffice/addIllnessToExistingPatient/${idIllness}/${idPatient}`)
  }
  curePatient(patientUserName){
    return this.http.get(`${this.URL}/MedicalOffice/CurePatient/${patientUserName}`)
  }
  addMedicine(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/MedicalOffice/AddMedicine`, body, {headers: token})
  }
  getPatientDetails(idPatient){
    return this.http.get(`${this.URL}/MedicalOffice/GetPatientDetails/${idPatient}`)
  }
  getPatientDoctorDetails(idDoctor){
    return this.http.get(`${this.URL}/MedicalOffice/GetPatientDoctorDetails/${idDoctor}`)
  }
  addPatientDetails(idPatient, idDetail, body){
    return this.http.post(`${this.URL}/MedicalOffice/AddPatientDetails/${idPatient}/${idDetail}`, body)
  }

}
