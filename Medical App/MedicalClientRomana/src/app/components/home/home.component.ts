import { Component, OnInit } from '@angular/core';
import { MedicalService } from 'src/app/services/medical.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IfStmt } from '@angular/compiler';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  logged: boolean;
  admin: boolean;
  userDetails: any = {
    userName: '',
    email: '',
    phoneNumber: '',
    role: '',
    id: ''
  };
  showUserDetails = false
  patients: any = null;
  users: Object;
  newPatient = {
    Name: '',
    Address: '',
    Date: '',
    MedicId: '',
    IllnessId: ''
  }
  newAppointment = {
    MedicId: null,
    PatientId: null,
    Type: '',
    Date: '',
    Hour: null,
    Minute: null
  }
  appointmentTypes = ['Consultatie', 'Tratament', 'Analize', 'Operatie']
  hours = [8,9,10,11,12,13,14,15,16,17,18,19,20]
  minutes = [0,10,15,20,30,40,45,50]
  addPatientZone = false
  illnesses: Object;
  currentMedicPatients: any;
  detailsZone = false;
  selectedPatient: boolean;
  selectedPatientDetails: any;
  selectedPatientSymptomes: any;
  appointments: any = null;
  appointmentAdded = false
  accountsZone: boolean = false;
  toDisplay: number = 0
  appointmentZone: boolean = false;
  patientZone: boolean = false;
  patientsSorted: boolean;
  searchName: string;
  noPatients: boolean = true;
  selectedPatientIllnessesAndTreatments: any;
  getSickZone: boolean = false;
  nextSickPatient: any;
  newIllToExistingPatient: any;
  patientNoIllness: boolean;

  constructor(private toastr: ToastrService, private service: MedicalService, private router: Router) { }

  ngOnInit() {
    //localStorage.removeItem('token')
    this.getUser()
    this.getUsers()
    this.getPatients()
    setTimeout( ()=> this.getAppointments(), 300);
    
    this.getIllnesses()
    this.getCurrentPatients()
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
        },
        err => {
          localStorage.removeItem('token')
          localStorage.removeItem('role')
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
  showUser(){
    if(this.showUserDetails == true)
      this.showUserDetails = false
    else
      this.showUserDetails = true
  }
  getPatients(){
    this.service.getPatients().subscribe(
      res => {
        this.patients = res
      }
    )
  }
  getCurrentPatients(){
    if(localStorage.getItem('token')){
      setTimeout( () => this.service.getCurrentPatients(this.userDetails.id).subscribe(
        res => {
          this.currentMedicPatients = res
          if(res != ''){
            this.noPatients = false
          }
        }
      ), 300)
    }
        
    
  }
  getAppointments(){
    this.service.getMedicAppointments(this.userDetails.id).subscribe(
      res =>{
        this.appointments = res
      }
    )
  }
  deleteAppointment(appointmentId){
    this.service.deleteAppointment(appointmentId).subscribe(
      (res: any) => {
        this.toastr.success(`Programare de tip ${res.type} stearsa cu succes!`,'Programare stearsa!')
        this.getAppointments()
      }
    )
  }
  getUsers(){
    this.service.getUsers().subscribe(
      res => {
        this.users = res
      }
    )
  }
  getIllnesses(){
    this.service.getIllnesses().subscribe(
      res => {
        this.illnesses = res
      }
    )
  }
  confirmPatient(){
    this.addPatientZone = true
    this.newPatient.MedicId = this.userDetails.id
  }
  addPatient(){
    if(this.newPatient.IllnessId == ''){
      this.newPatient.IllnessId = '0'
    }
    this.service.addPatient(this.newPatient).subscribe(
      res => {
        this.toastr.success(`Pacient ${this.newPatient.Name} adaugat!`)
      }
    )
    //in order to execute this request after the conclusion of the first one (async functions in controller may take longer in some cases milliseconds matter)
    setTimeout( ()=> this.service.addPatientToHistory(this.newPatient.Name, this.newPatient.IllnessId).subscribe(
      res => {
        this.getCurrentPatients()
        this.newPatient.Name = ''
        this.newPatient.IllnessId = ''
        this.newPatient.Address = ''
        this.newPatient.Date = ''
        this.newPatient.MedicId = ''
        this.addPatientZone = false
      }
    ), 50);
    
  }
  addAppointment(){
    this.newAppointment.MedicId = this.userDetails.id
    this.service.addAppointment(this.newAppointment).subscribe(
      res => {
        this.toastr.success(`Programare noua adaugata!`, 'Adaugat cu succes!')
        setTimeout(() => this.getAppointments(), 100)
        
        this.appointmentAdded = true
        setTimeout( () => this.appointmentAdded = false, 50)
        this.newAppointment.PatientId = null
        this.newAppointment.MedicId = ''
        this.newAppointment.Date = ''
        this.newAppointment.Hour = null
        this.newAppointment.Minute = null
        this.newAppointment.Type = ''
      }
    )
  }
  deletePatient(idPatient){
    this.service.deletePatient(idPatient).subscribe(
      (res: any) => {
        this.toastr.success(`Pacientul ${res.name} a fost sters!`,'Sters cu succes!')
        this.getPatients()
        this.getCurrentPatients()        
        setTimeout( () => {if(this.currentMedicPatients.length <= 1){
          this.noPatients = true
        }}, 100)
        
        
      },
      err => {
        this.toastr.error(`Pacientul are programari neconcluzionate!`,'Eroare!')
      }
    )
  }
  cencelAdd(){
    this.addPatientZone = false
    this.newPatient.Name = ''
    this.newPatient.IllnessId = ''
    this.newPatient.Address = ''
    this.newPatient.Date = ''
    this.newPatient.MedicId = ''
    this.getSickZone = false
    this.nextSickPatient = ''
  }
  details(){
    if(!this.detailsZone)
      this.detailsZone = true
    else{
      this.detailsZone = false
      this.selectedPatient = false
    }
  }
  selectPatient(patientId){
    this.selectedPatientDetails = ''
    this.selectedPatientSymptomes = null
    this.service.getPatient(patientId).subscribe(
      (res: any) => {
        if(res.length == 0){
          this.patientNoIllness = true
        }
        this.selectedPatient = true
        this.selectedPatientDetails = res[0]
        this.selectedPatientIllnessesAndTreatments = res
      },
      err =>{
        
      }
    )
  }
  healed(idHistoric, idPatient, idIllness){
    this.service.healed(idHistoric, idPatient, idIllness).subscribe(
      res => {
        this.selectPatient(idPatient)
        this.toastr.success(`Pacient vindecat!`)
      }
    )
  }
  showOption(display){
    if(this.toDisplay == 0){
      this.toDisplay = display
      console.log(this.toDisplay)}
    else
      this.toDisplay = 0
    setTimeout(() => window.scrollBy({top: 1000, behavior: 'smooth'}), 10)
  }
  enterAccounts(){
    if(!this.accountsZone)
      this.accountsZone = true
    else
      this.accountsZone = false
    setTimeout(() => window.scrollBy({top: 1000, behavior: 'smooth'}), 10)
  }
  addAppointmentZone(){
    if(this.appointmentZone == false)
      this.appointmentZone = true
    else
      this.appointmentZone = false
  }
  addPatientsZone(){
    if(this.patientZone == false)
      this.patientZone = true
    else
      this.patientZone = false
  }
  getSick(patientName){
    this.getSickZone = true
    this.patientNoIllness = false
    this.nextSickPatient = patientName
  }
  addIllToExistingPatient(){
    this.service.addPatientToHistory(this.nextSickPatient, this.newIllToExistingPatient).subscribe(
      res => {
        this.getCurrentPatients()
        this.toastr.success(`Boala adaugata pacientului ${this.nextSickPatient}`, 'Adaugat cu succes!')
        this.nextSickPatient = ''
        this.getSickZone = false
      }
    )
  }
  sortPatients(){
    if(!this.patientsSorted){
      this.patientsSorted = true
      this.currentMedicPatients.sort((a,b) => (a.name > b.name) ? 1 : ((b.name > a.name) ? -1 : 0)); 
    }
    else{
      this.patientsSorted = false
      this.getCurrentPatients()
    }
  }
  Search(){
    if(this.searchName == ""){
      this.getCurrentPatients()
    }
    else
      this.currentMedicPatients = this.currentMedicPatients.filter( res => {
        return res.name.toLocaleLowerCase().match(this.searchName.toLocaleLowerCase())
      })
  }
  scrollUp(){
    setTimeout(() => this.toDisplay = 0, 600)
    setTimeout(() => this.accountsZone = false, 400)
    setTimeout(() => window.scrollBy({top: -10000, behavior: 'smooth'}), 10)

    this.newAppointment.PatientId = null
    this.newAppointment.MedicId = ''
    this.newAppointment.Date = ''
    this.newAppointment.Hour = null
    this.newAppointment.Minute = null
    this.newAppointment.Type = ''
    this.newPatient.Name = ''
    this.newPatient.IllnessId = ''
    this.newPatient.Address = ''
    this.newPatient.Date = ''
    this.newPatient.MedicId = ''
    this.patientZone = false
    this.appointmentZone = false
  }
}
