import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MedicalService } from 'src/app/services/medical.service';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  users: Object;
  patients: any;
  appointments: Object;
  mismatch: boolean;
  currentUser: Object = {
    userName: '',
    email: '',
    phoneNumber: '',
    id: '',
    role: ''
  };
  symptomes: Object;
  severities = ['scazuta', 'medie', 'ridicata']
  medicaments: Object;
  treatmentZone = 1;
  illnesses: any;
  currentIll: any;
  currentSymptom: any
  displayAddedSymptoms = []

  currentPatient = {
    idPatient: null,
    name: '',
    medicId: null,
    medicName: ''
  };
  nextMedic = {
    id: null,
    name: ''
  };
  patientSelected: boolean = false;
  nextMedicSelected: boolean = false;
  transfered: boolean = false;
  toDisplay: number = 0;
  historic: Object;
  medicamentZone: boolean = false;
  illnessZone: boolean = false;
  userZone: boolean = false;
  medics: Object;

  constructor(private fb: FormBuilder, private toastr: ToastrService, private service: MedicalService, private router: Router) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    PhoneNumber: [''],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['', Validators.required],
    Seniority: 0
  })
  
  roles = ['Admin', 'Medic']

  medicament = { 
    Name: '',
    Price: null
  }

  illness = {
    Name: '',
    Severity: ''
  }

  treatment = {
    IllnessId: '',
    MedicamentId: '',
    Duration: '',
    Quantity: '',
    PillPerDay: '',
    DayTime: ''
  }
  
  ngOnInit() {
    this.getUsers()
    this.getMedics()
    this.getCurrentUser()
    this.getPatients()
    this.getAppointments()
    this.getSymptomes()
    this.getMedicaments()
    this.getIllnesses()
    this.getHistoric()
  }
  comparePasswords(){
    if(this.formModel.value.Password != this.formModel.value.ConfirmPassword){
      this.mismatch = true
    }
    else{
      this.mismatch = false
    }
  }

  getCurrentUser(){
    this.service.getUser().subscribe(
      res => {
        this.currentUser = res
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
  getMedics(){
    this.service.getMedics().subscribe(
      res => {
        this.medics = res
      }
    )
  }
  getSymptomes(){
    this.service.getSymptomes().subscribe(
      res => {
        this.symptomes = res
      }
    )
  }
  getMedicaments(){
    this.service.getMedicaments().subscribe(
      res => {
        this.medicaments = res
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
  getHistoric(){
    this.service.getHistoric().subscribe(
      res => {
        this.historic = res
      }
    )
  }
  deleteUser(userId){
    this.service.deleteUser(userId).subscribe(
      (res: any) => {
        this.getUsers()
        this.toastr.success(`Utilizatorul ${res.userName} a fost sters!`,'Sters cu succes!')
      }
    )
  }
  deleteAppointment(appointmentId){
    this.service.deleteAppointment(appointmentId).subscribe(
      (res: any) => {
        this.toastr.success(`Programarea de tip ${res.type} a fost stearsa cu succes!`,'Programare stearsa!')
        this.getAppointments()
      }
    )
  }
  getPatients(){
    this.service.getPatients().subscribe(
      res => {
        this.patients = res
      }
    )
  }
  getAppointments(){
    this.service.getAppointments().subscribe(
      res => {
        this.appointments = res
      }
    )
  }
  getPatientCurrentMedic(idPatient){
    this.service.getPatientCurrentMedic(idPatient).subscribe(
      (res: any) => {
        this.patientSelected = true
        this.currentPatient = res
        console.log(res)
      }
    )
  }
  selectNextMedic(idUser, userName){
    this.nextMedic.id = idUser
    this.nextMedic.name = userName
    this.patientSelected = false
    this.nextMedicSelected = true
  }
  transferPatient(){
    if(this.nextMedic.id == this.currentPatient.medicId)
    {
      this.toastr.error('Medicul curent este identic cu cel pentru transfer','Intrari invalide')
    }
    else{
      this.service.transferPatient(this.currentPatient.idPatient, this.nextMedic.id).subscribe(
        res => {
          this.toastr.success(`Pacientul ${this.currentPatient.name} a fost transferat de la medicul ${this.currentPatient.medicName} la  medicul ${this.nextMedic.name}!`, 'Transfer cu succes')
          this.getPatients()
          this.transfered = false
          this.patientSelected = false
          this.nextMedicSelected = false
          this.currentPatient.name = ''
          this.currentPatient.medicId = null
          this.currentPatient.medicName = ''
          this.nextMedic.id = null
          this.nextMedic.name = ''
          //location.reload();
        }
      )
      
    }
  }
  addMedicament(){
    if(this.medicament.Price >= 1){
      this.service.addMedicament(this.medicament).subscribe(
        res => {
          this.toastr.success(`Medicamentul ${this.medicament.Name} adaugat`,'Adaugat cu succes!')
          this.medicament.Name = ''
          this.medicament.Price = null
          this.getMedicaments()
        },
        err => {
          console.log(err)
          this.toastr.error(`Medicamentul ${this.medicament.Name} nu a fost adaugat`,'Adaugare esuata!')
        }
      )
    }
    else{
      this.toastr.error('Pretul unui medicament nu ar trebui sa fie mai mic decat 1 leu', 'Pret invalid!')
    }
  }
  addIllness(){
    this.service.addIllness(this.illness).subscribe(
      res => {
        this.toastr.success(`Boala ${this.illness.Name} adaugata`,'Adaugat cu succes!')
        this.illness.Name = ''
        this.illness.Severity = ''
        this.treatmentZone = 2
        setTimeout( () => this.service.getLastIllness().subscribe(
          res => {
            this.currentIll = res
          }
        ), 200)
        setTimeout(() => {
          this.getIllnesses()
        }, 200);
      },
      err => {
        console.log(err)
        this.toastr.success(`Boala ${this.illness.Name} nu a fost adaugata`,'Adaugare esuata!')
      }
    )
  }
  addTreatment(){
    this.treatment.IllnessId = this.currentIll.id
    this.service.addTreatment(this.treatment).subscribe(
      res => {
        this.toastr.success('Tratament adaugat')
        this.treatmentZone = 3
      }
    )
  }
  addSymptom(){
    console.log(this.currentIll.id, this.currentSymptom)
    this.service.addSymptom(this.currentIll.id, this.currentSymptom).subscribe(
      res => {
        this.toastr.success(`Simptom adaugat cu succes pentru boala ${this.currentIll.description}!`,'Adaugat cu succes!')
        this.service.getSymptom(this.currentSymptom).subscribe(
          (res: any) => {
            this.displayAddedSymptoms.push(res.description)
          }
        )
        
      }
    )
  }
  Finish(){
    this.currentSymptom = null
    this.currentIll = null
    this.displayAddedSymptoms = []
    this.treatmentZone = 1
    this.medicament.Name = ''
    this.medicament.Price = null
    this.illness.Name = ''
    this.illness.Severity = ''
    this.treatment.IllnessId = null
    this.treatment.MedicamentId = null
    this.treatment.Duration = null
    this.treatment.Quantity = null
    this.treatment.PillPerDay = null
    this.treatment.DayTime = ''
  }
  deletePatient(idPatient){
    this.service.deletePatient(idPatient).subscribe(
      (res: any) => {
        this.toastr.success(`Pacientul ${res.name} a fost sters!`,'Sters cu succes!')
        this.getPatients()
      },
      err => {
        this.toastr.error(`Pacientul are programari neconcluzionate!`,'Eroare!')
      }
    )
  }
  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      PhoneNumber: this.formModel.value.PhoneNumber,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role,
      Seniority: this.formModel.value.Seniority
    }
    console.log(body)
    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`Utilizatorul ${body.UserName} a fost creat cu succes`,'Inregistrat cu succes!')
          this.formModel.reset()
          //this.formModel.value.Role = 'Medic'
          this.getUsers()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username existent', 'Inregistrare esuata!')
            else
              this.toastr.error(`${element.description}`, 'Inregistrare esuata!')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }
  checkSeniority(){
    if(this.formModel.value.Role == 'Medic' && this.formModel.value.Seniority == 0){
      return false
    }
    else
      return true;
  }
  showOption(display){
    if(this.toDisplay == 0)
      this.toDisplay = display
    else
      this.toDisplay = 0
    setTimeout(() => window.scrollBy({top: 1200, behavior: 'smooth'}), 10)
  }
  addMedicamentZone(){
    if(this.medicamentZone == false)
      this.medicamentZone = true
    else
      this.medicamentZone = false
  }
  addIllnessZone(){
    if(this.illnessZone == false)
      this.illnessZone = true
    else
      this.illnessZone = false
  }
  addUserZone(){
    if(this.userZone == false)
      this.userZone = true
    else
      this.userZone = false
  }
  scrollUp(){
    setTimeout(() => this.toDisplay = 0, 600)
    setTimeout(() => window.scrollBy({top: -10000, behavior: 'smooth'}), 10)

    this.patientSelected = false
    this.nextMedicSelected = false
    this.medicamentZone = false
    this.currentPatient.name = ''
    this.currentPatient.medicId = null
    this.currentPatient.medicName = ''
    this.nextMedic.id = null
    this.nextMedic.name = ''
    this.medicament.Name = ''
    this.medicament.Price = null
    this.illness.Name = ''
    this.illness.Severity = ''
  }
}
