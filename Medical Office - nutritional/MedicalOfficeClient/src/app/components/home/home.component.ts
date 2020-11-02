import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MedicalOfficeService } from 'src/app/services/medical-office.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userDetails: any ={
    id: ''
  };
  currentPatients: any;
  logged: boolean;
  loggedUser: string;
  users: any;
  verifiedUser: string;
  appointments: any;
  patients: any;
  medicaments: any;
  illnesses: any;

  addMedicamentZone: boolean = false
  addAppointmentZone: boolean = false
  addUserZone: boolean = false
  addIllnessesZone: boolean = false
  addPatientZone: boolean = false
  medicament = { 
    Name: '',
    Price: null
  }

  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  illnessForm: FormGroup;
  illnessDetailsForm: FormGroup;

  isLinear = true
  hours = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18]
  minutes = [0, 10, 15, 20, 30, 40, 45, 50]
  doctors: any;

  formModel = this.formBuilder.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Address: [''],
    PhoneNumber: [''],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['', Validators.required],
    IllnessId: ['']
  })
  roles: string[] = ['Admin', 'Doctor']
  mismatch: boolean;

  illnessForms = this.formBuilder.group({
    Name: ['', Validators.required],
    Symptoms: ['', Validators.required],
    Risk:['', Validators.required]
  })
  risks: string[] = ['Low', 'Medium', 'High', 'Deadly']
  currentAppointments: any;
  addAppointmentDoctor: boolean;
  selectedPatientAppointment: any;
  newDoctorAppointment: any;
  patientIllMedicine: Object;
  historic: any = {
    date: '',
    hour: null,
    idMedic: '',
    idPacient: '',
    minute: null,

  };
  addIllnessToPatient: boolean;
  selectedPatientIll: any;
  selectedPatientIllId: any;
  selectedIllForPatient: any = '';
  currentPatientAppointments: any = '';

  bodyType = ['Slim', 'Fit', 'Robust']
  activity = ['Sedentary', 'Light', 'Moderate', 'Heavy', 'Intense']
  patientDetailsForm = this.formBuilder.group({
    Height: [null, Validators.required],
    Weight: [null, Validators.required],
    Age:[null, Validators.required],
    bodyType:['', Validators.required],
    Activity: ['', Validators.required]
  })
  patientDetails: any = {
  };
  patientHasDetails: boolean = false;
  updateZone: boolean = false;
  patientDetailsObj: any = {
    Height: null,
    Weight: null,
    Age: null,
    bodyType: '',
    Activity: ''
  };
  patientDoctorDetails: any
  addDetailsDoctor: boolean = false;
  selectedPatientDetailId: any;
  currentDetail: any;
  edit: boolean = false;
  showDetails: boolean = false;
  patientShowDetails: boolean = false;
  selectedPatientDoctorDetails: Object;
  currentPacientNutritionistDetails: any;
  
  constructor(private formBuilder: FormBuilder, private router: Router, private service: MedicalOfficeService, private toastr: ToastrService) { }

  ngOnInit() {
    //localStorage.removeItem('token')
    //localStorage.removeItem('role')
    this.getUser()
    this.getUsers()
    this.getAppointments()
    this.getMedicaments()
    this.getIllnesses()
    setTimeout( () => this.getHistoric(), 300)
    this.getDoctors()
    this.getPatients()
    setTimeout( () => this.getCurrentPatients(), 300)
    setTimeout( () => this.getPatientIllMedicine(), 300)
    setTimeout( () => this.getCurrentAppointments(), 300)
    setTimeout( () => this.getPatientAppointments(), 300)
    setTimeout( () => this.getPatientDetails(), 400)
    setTimeout( () => this.getPatientDoctorDetails(), 300)
    

    this.illnessForm = this.formBuilder.group({
      Name: ['', Validators.required],
      Symptoms: ['', Validators.required],
      Risk:['', Validators.required]
    });
    this.illnessDetailsForm = this.formBuilder.group({
      Duration: ['', Validators.required],
      DailyDose: ['', Validators.required],
      Period:['', Validators.required],
      //IllnessId:['', Validators.required],
      MedicamentId:['', Validators.required]
    });
    this.firstFormGroup = this.formBuilder.group({
      Doctor: ['', Validators.required],
      Patient: ['', Validators.required]
    });
    this.secondFormGroup = this.formBuilder.group({
      Date: ['', Validators.required],
      Hour: [ null, Validators.required],
      Minute: [ null, Validators.required]
    });

  }
  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    this.router.navigateByUrl('/user/login')
  }
  login(){
    this.router.navigateByUrl('/user/login')
  }
  register(){
    this.router.navigateByUrl('/user/register')
  }
  registerUser(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Address: this.formModel.value.Address,
      PhoneNumber: this.formModel.value.PhoneNumber,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role
    }
    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} successfully created`,'Registration succeeded')
          this.formModel.reset()
          this.cancelZone()
          this.getUsers()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Username taken', 'Registration failed!')
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }
  addPatient(){
    if(!this.formModel.value.IllnessId){
      this.formModel.value.IllnessId = 0
    }
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Address: this.formModel.value.Address,
      PhoneNumber: this.formModel.value.PhoneNumber,
      Password: this.formModel.value.Password,
      Role: 'Pacient',
      IdDoctor: this.userDetails.id,
      IllnessId: this.formModel.value.IllnessId
    }
    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.toastr.success(`Patient ${body.UserName} successfully added`,'Add patient succeeded')
          this.formModel.reset()
          this.cancelZone()
          this.getCurrentPatients()
          this.getPatientIllMedicine()
        }
        else {
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName')
              this.toastr.error('Patient name taken taken', 'Add patient failed!')
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
      },
      err => {
        console.log(err)
      }
    )
  }
  comparePasswords(){
    if(this.formModel.value.Password != this.formModel.value.ConfirmPassword){
      this.mismatch = true
    }
    else{
      this.mismatch = false
    }
  }
  getUsers(){
    this.service.getUsers().subscribe(
      res => {
        this.users = res
      }
    )
  }
  
  getDoctors(){
    this.service.getDoctors().subscribe(
      (res) => {
        this.doctors = res
      }
    )
  }
  getPatients(){
    this.service.getPatients().subscribe(
      (res) => {
        this.patients = res
      }
    )
  }
  getCurrentPatients(){
    if(this.loggedUser == 'Doctor'){
      this.service.getCurrentPatients(this.userDetails.id).subscribe(
        (res: any) => {
          this.currentPatients = res
        }
      )

    }
  }
  verifyUser(id){
    this.service.VerifyUser(id).subscribe(
      res => {
        this.verifiedUser = 'Admin';
      },
      err =>{
        console.log(err)
      }
    )
  }
  getUser(){
    if(localStorage.getItem('token') != null){
      this.service.getUser().subscribe(
        (res: any) => {
          this.userDetails = res
          this.logged = true
          localStorage.setItem('role', res.role)

          switch(localStorage.getItem('role')){
            case "Admin":
              this.loggedUser = "Admin"
              break;
            case "Doctor":
              this.loggedUser = "Doctor"
              break;
            case "Pacient":
              this.loggedUser = "Pacient"
              break;
            default:
              break;
          }
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
  deleteUser(userId){
    this.service.deleteUser(userId).subscribe(
      (res: any) => {
        this.getUsers()
        this.getCurrentPatients()
        this.getPatientIllMedicine()
        this.getPatientAppointments()
        this.getPatientDoctorDetails()
        this.getCurrentAppointments()
        this.getAppointments()
        this.toastr.success(`User ${res.userName} has been deleted!`,'Delete successfull!')
      }
    )
  }
  getAppointments(){
    this.service.getAppointments().subscribe(
      (res: any) => {
        this.appointments = res
      }
    )
  }
  getCurrentAppointments(){
    if(this.loggedUser == 'Doctor'){
      this.service.getCurrentAppointments(this.userDetails.id).subscribe(
        (res: any) => {
          this.currentAppointments = res
        }
      )
    }
  }
  getPatientAppointments(){
    if(this.loggedUser == 'Pacient'){
      this.service.getPatientAppointments(this.userDetails.id).subscribe(
        (res: any) => {
          this.currentPatientAppointments = res
        }
      )
    }
  }
  addAppointment(){
    var appointment = {
      MedicId: this.firstFormGroup.value.Doctor,
      PatientId: this.firstFormGroup.value.Patient,
      Date: this.secondFormGroup.value.Date,
      Hour: this.secondFormGroup.value.Hour,
      Minute: this.secondFormGroup.value.Minute,
    }
    this.service.addAppointment(appointment).subscribe(
      res => {
        this.addAppointmentZone = false
        this.firstFormGroup.reset()
        this.secondFormGroup.reset()
        this.getAppointments()
      }
    )
  }
  addDoctorAppointment(){
    this.service.addAppointment(this.newDoctorAppointment).subscribe(
      res => {
        this.toastr.success(`Appointment for patient ${this.selectedPatientAppointment} assigned`, 'Apointment added')
        this.selectedPatientAppointment = ''
        this.addAppointmentDoctor = false
        this.newDoctorAppointment = {
          MedicId: '',
          PatientId: '',
          Date: '',
          Hour: null,
          Minute: null,
        }
        this.getCurrentAppointments()
      }
    )
  }
  patientAppointmentZone(idPacient, namePatient){
    this.selectedPatientAppointment = namePatient
    this.addAppointmentDoctor = true
    this.addIllnessToPatient = false
    this.addDetailsDoctor = false
    this.addPatientZone = false
    this.newDoctorAppointment = {
      MedicId: this.userDetails.id,
      PatientId: idPacient
    }
  }
  selectIll(patinetId, namePatient){
    this.selectedPatientIll = namePatient
    this.selectedPatientIllId = patinetId
    this.addIllnessToPatient = true
    this.addDetailsDoctor = false
    this.addAppointmentDoctor = false
  }
  deleteAppointment(appointmentId){
    this.service.deleteAppointment(appointmentId).subscribe(
      (res: any) => {
        this.toastr.success('Appointment done!')
        this.getCurrentAppointments()
        this.getAppointments()
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
  getPatientIllMedicine(){
    this.service.getPatientIllnesses(this.userDetails.id).subscribe(
      res => {
        this.patientIllMedicine = res
      }
    )
  }
  getHistoric(){
    if(localStorage.getItem('role') == "Admin"){
      this.service.getHistoric().subscribe(
        res => {
          this.historic = res
        }
      )
    }
  }
  addMedicament(){
    if(this.medicament.Price >= 1){
      this.service.addMedicament(this.medicament).subscribe(
        res => {
          this.toastr.success(`Medicament ${this.medicament.Name} added successfully`,'Add successfull!')
          this.medicament.Name = ''
          this.medicament.Price = null
          this.getMedicaments()
          this.medicament.Name = ''
          this.medicament.Price = null
          this.addMedicamentZone = false
        },
        err => {
          console.log(err)
          this.toastr.error(`Medicament ${this.medicament.Name} not added`,'Add failedd')
        }
      )
    }
    else{
      this.toastr.error('Price of a medicament shouldn\'t be under 1', 'Invalid price!')
    }
  }
  addIllness(){
    var body = {
      Name: this.illnessForm.value.Name,
      Symptoms: this.illnessForm.value.Symptoms,
      Risk: this.illnessForm.value.Risk
    }
    this.service.addIllness(body).subscribe(
      res => {
        this.toastr.success(`Illness ${this.illnessForm.value.Name} added!`, 'Add successful!')
        this.getIllnesses()
        var body = {
          Duration: this.illnessDetailsForm.value.Duration,
          DailyDose: this.illnessDetailsForm.value.DailyDose,
          Period: this.illnessDetailsForm.value.Period,
          IllnessId: res,
          MedicamentId: this.illnessDetailsForm.value.MedicamentId
        }
        this.service.addMedicine(body).subscribe(
          res => {
            this.getIllnesses()
            this.cancelZone()
          }
        )
      }
    )
  }
  addIllnessToExistingPatient(){
    this.service.addIllnessToExistingPatient(this.selectedIllForPatient, this.selectedPatientIllId).subscribe(
      (res: any) => {
        this.getPatientIllMedicine()
        this.toastr.success(`Illness ${res.name} added to patient ${this.selectedPatientIll}`, `Illness added successful!`)
        this.cancelZone()
      }
    )
  }
  curePatient(patientUserName){
    this.service.curePatient(patientUserName).subscribe(
      res => {
        this.toastr.success(`Patient ${patientUserName} cured`)
        this.getPatientIllMedicine()
      }
    )
  }
  getPatientDetails(){
    if(localStorage.getItem('role') == 'Pacient'){
      this.service.getPatientDetails(this.userDetails.id).subscribe(
        res => {
          this.patientHasDetails = true
          this.patientDetails = res
        }
      )
    }
  }
  getPatientDoctorDetails(){
    if(localStorage.getItem('role') == 'Doctor'){
      this.service.getPatientDoctorDetails(this.userDetails.id).subscribe(
        res => {
          this.patientHasDetails = true
          this.patientDoctorDetails = res
        }
      )
    }
  }
  getOnePatientDoctorDetails(idPacient, numePacient){
    if(localStorage.getItem('role') == 'Doctor'){
      this.service.getPatientDetails(idPacient).subscribe(
        res => {
          this.patientShowDetails = true
          this.patientDetails = res
          this.currentPacientNutritionistDetails = numePacient
          setTimeout(() => {
            window.scrollBy({top: 1000, behavior: 'smooth'})
          }, 100);
        }
      )
    }
  }
  addPatientDetails(){
    if(!this.patientDetails.id) this.patientDetails.id = 0
    this.service.addPatientDetails(this.userDetails.id, this.patientDetails.id, this.patientDetailsForm.value).subscribe(
      res => {
        this.toastr.success(`Details added successful!`)
        this.cancelZone()
        this.getPatientDetails()
      }
    )
  }
  addPatientDoctorDetails(){
    if(!this.currentDetail) this.currentDetail = 0
    this.service.addPatientDetails(this.selectedPatientDetailId, this.currentDetail, this.patientDetailsForm.value).subscribe(
      res => {
        this.toastr.success(`Details added successful!`)
        this.cancelZone()
        this.getPatientDoctorDetails()
      },
      err => {
        this.toastr.error(`This patient already have details!`)
        this.cancelZone()
      }
    )
  }
  addDetailDoctorZone(patientId, namePatient, patient){
    this.addDetailsDoctor = true
    this.addAppointmentDoctor = false
    this.addIllnessToPatient = false
    this.addPatientZone = false
    this.selectedPatientAppointment = namePatient
    this.selectedPatientDetailId = patientId
  }
  updateDetails(){
    this.updateZone = true
    this.patientDetailsForm.value.Height = this.patientDetails.height
    this.patientDetailsForm.value.Weight = this.patientDetails.weight
    this.patientDetailsForm.value.Age = this.patientDetails.age
    this.patientDetailsForm.value.bodyType = this.patientDetails.bodyType
    this.patientDetailsForm.value.Activity = this.patientDetails.activity
    this.patientDetailsObj.Height = this.patientDetails.height
    this.patientDetailsObj.Weight = this.patientDetails.weight
    this.patientDetailsObj.Age = this.patientDetails.age
    this.patientDetailsObj.bodyType = this.patientDetails.bodyType
    this.patientDetailsObj.Activity = this.patientDetails.activity
  }
  updateDetailsDoctor(idDetailPacient, idDetail, patient){
    this.edit = true
    this.updateZone = true
    this.patientDetailsForm.value.Height = patient.height
    this.patientDetailsForm.value.Weight = patient.weight
    this.patientDetailsForm.value.Age = patient.age
    this.patientDetailsForm.value.bodyType = patient.bodyType
    this.patientDetailsForm.value.Activity = patient.activity
    this.patientDetailsObj.Height = patient.height
    this.patientDetailsObj.Weight = patient.weight
    this.patientDetailsObj.Age = patient.age
    this.patientDetailsObj.bodyType = patient.bodyType
    this.patientDetailsObj.Activity = patient.activity
    this.currentDetail = idDetail
    this.selectedPatientDetailId = idDetailPacient
    setTimeout(() => {
      window.scrollBy({top: 1000, behavior: 'smooth'})
    }, 100);
  }
  addZone(option){
    switch(option){
      case 'User':
        this.addUserZone = true
        this.addAppointmentZone = false
        this.addMedicamentZone = false
        this.addIllnessesZone = false
        this.addPatientZone = false
        this.addIllnessToPatient = false
        this.addDetailsDoctor = false
        break;
      case 'Appointment':
        this.addUserZone = false
        this.addAppointmentZone = true
        this.addMedicamentZone = false
        this.addIllnessesZone = false
        this.addPatientZone = false
        this.addIllnessToPatient = false
        this.addDetailsDoctor = false
        break;
      case 'Medicament':
        this.addUserZone = false
        this.addAppointmentZone = false
        this.addMedicamentZone = true
        this.addIllnessesZone = false
        this.addPatientZone = false
        this.addIllnessToPatient = false
        this.addDetailsDoctor = false
        break;
      case 'Illness':
        this.addUserZone = false
        this.addAppointmentZone = false
        this.addMedicamentZone = false
        this.addIllnessesZone = true
        this.addPatientZone = false
        this.addIllnessToPatient = false
        this.addDetailsDoctor = false
        break;
      case 'AddPatient':
        this.addUserZone = false
        this.addAppointmentZone = false
        this.addMedicamentZone = false
        this.addIllnessesZone = false
        this.addPatientZone = true
        this.addIllnessToPatient = false
        this.addDetailsDoctor = false
        break;
      case 'AddDetails':
        this.addUserZone = false
        this.addAppointmentZone = false
        this.addMedicamentZone = false
        this.addIllnessesZone = false
        this.addPatientZone = true
        this.addIllnessToPatient = false
        this.addDetailsDoctor = true
        break;
      default:
        break;
    }
  }
  cancelZone(){
    this.addUserZone = false
    this.addAppointmentZone = false
    this.addMedicamentZone = false
    this.addIllnessesZone = false
    this.addPatientZone = false
    this.addAppointmentDoctor = false
    this.addIllnessToPatient = false
    this.updateZone = false
    this.addDetailsDoctor = false
    this.edit = false
    this.patientShowDetails = false
    
    this.selectedPatientAppointment = ''
    this.currentPacientNutritionistDetails = ''
    this.selectedIllForPatient = ''
    this.selectedPatientIll = ''

    this.medicament.Name = ''
    this.medicament.Price = null
    this.formModel.reset()
    this.illnessForms.reset()
    this.illnessForm.reset()
    this.illnessDetailsForm.reset()
    this.firstFormGroup.reset()
    this.secondFormGroup.reset()
    this.patientDetailsForm.reset()
  }
  nutritionalDetailsZone(){
    if(this.showDetails)
      this.showDetails = false
    else 
      this.showDetails = true
    setTimeout(() => {
      window.scrollBy({top: 1000, behavior: 'smooth'})
    }, 100);
  }
}
