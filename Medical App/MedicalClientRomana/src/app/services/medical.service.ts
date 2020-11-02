import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class MedicalService {
  URL = 'http://localhost:51775/api'
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
  getPatients(){
    return this.http.get(`${this.URL}/Medical/GetPatients`)
  }
  getPatient(idPatient){
    return this.http.get(`${this.URL}/Medical/GetPatient/${idPatient}`)
  }
  getAppointments(){
    return this.http.get(`${this.URL}/Medical/Appointments`)
  }
  getMedicAppointments(idMedic){
    return this.http.get(`${this.URL}/Medical/MedicAppointments/${idMedic}`)
  }
  getSymptomes(){
    return this.http.get(`${this.URL}/Medical/Symptoms`)
  }
  getMedicaments(){
    return this.http.get(`${this.URL}/Medical/Medicaments`)
  }
  getHistoric(){
    return this.http.get(`${this.URL}/Medical/GetHistoric`)
  }
  getCurrentPatients(idMedic){
    return this.http.get(`${this.URL}/Medical/GetCurrentPatients/${idMedic}`)
  }
  getPatientCurrentMedic(idPatient){
    return this.http.get(`${this.URL}/Medical/GetPatientCurrentMedic/${idPatient}`)
  }
  getUsers(){
    return this.http.get(`${this.URL}/User/GetUsers`)
  }
  getMedics(){
    return this.http.get(`${this.URL}/User/GetMedics`)
  }
  getIllnesses(){
    return this.http.get(`${this.URL}/Medical/GetIllnesses`)
  }
  getIllness(idIllness){
    return this.http.get(`${this.URL}/Medical/GetIllness/${idIllness}`)
  }
  getLastIllness(){
    return this.http.get(`${this.URL}/Medical/GetLastIllness`)
  }
  getSymptom(idSymptom){
    return this.http.get(`${this.URL}/Medical/Symptom/${idSymptom}`)
  }
  addSymptom(idIllness, idSymptom){
    return this.http.get(`${this.URL}/Medical/AddSymptom/${idIllness}/${idSymptom}`)
  }
  addTreatment(body){
    return this.http.post(`${this.URL}/Medical/AddTreatment`, body)
  }
  addPatient(body){
    return this.http.post(`${this.URL}/Medical/AddPatient`, body)
  }
  addAppointment(body){
    return this.http.post(`${this.URL}/Medical/AddAppointment`, body)
  }
  addPatientToHistory(patientName, illnessId){
    return this.http.get(`${this.URL}/Medical/AddPatientToHistory/${patientName}/${illnessId}`)
  }
  addMedicament(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Medical/AddMedicament`, body, {headers: token})
  }
  addIllness(body){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.post(`${this.URL}/Medical/AddIllness`, body, {headers: token})
  }
  transferPatient(idPatient, idMedic){
    return this.http.get(`${this.URL}/Medical/TransferPatient/${idPatient}/${idMedic}`)
  }
  healed(idHistoric, idPatient, idIllness){
    return this.http.get(`${this.URL}/Medical/HealPatient/${idHistoric}/${idPatient}/${idIllness}`)
  }
  deleteUser(userId){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeleteUser/${userId}`, {headers: token})
  }
  deletePatient(idPatient){
    var token = new HttpHeaders({'Authorization': 'Bearer '+ localStorage.getItem('token')})
    return this.http.delete(`${this.URL}/User/DeletePatient/${idPatient}`, {headers: token})
  }
  deleteAppointment(appointmentId){
    return this.http.delete(`${this.URL}/Medical/DeleteAppointment/${appointmentId}`)
  }
}
