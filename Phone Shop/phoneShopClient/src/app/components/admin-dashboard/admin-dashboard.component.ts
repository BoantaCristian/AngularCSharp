import { Component, OnInit } from '@angular/core';
import { PhoneShopService } from 'src/app/services/phone-shop.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  users: any;
  passwordsMatch: boolean;
  phones: Object;
  companies: Object;
  detailsPhoneZone = 1;
  imagePath: string = "../../../assets/default-image.png"
  fileToUpload: File = null

  newPhone: any = {
    Name: '',
    LaunchDate: '',
    Price: null,
    ImagePath: '',
    CompanyId: null
  }

  newPhoneDetails: any = {
    Dimensions: '',
    Weight: '',
    DisplayType: '',
    Resolution: '',
    OS: '',
    MainCamera: '',
    SelfieCamera: '',
    Battery: ''
  }
  updatePhone: any = {
    newName: '',
    newCompany: '',
    newLaunchDate: '',
    newPrice: ''
  }
  newCompany: any = {
    Name: '',
    Description: '',
    LaunchDate: ''
  }
  currentEditPhone: any;
  historic: any;
  
  constructor(private service: PhoneShopService, private toastr: ToastrService, private fb: FormBuilder) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: ['', Validators.required]
  })
  roles = ['Admin', 'Client']

  ngOnInit() {
    this.getUsers()
    this.getPhonesWithCompanies()
    this.getCompanies()
    this.getHistoric()
  }

  getUsers(){
    this.service.getUsers().subscribe(
      res =>{
        this.users = res
      }
    )
  }
  getPhonesWithCompanies(){
    this.service.getPhonesWithCompanies().subscribe(
      res =>{
        this.phones = res
      }
    )
  }
  getCompanies(){
    this.service.getCompanies().subscribe(
      (res: any) => {
        this.companies = res
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
  deleteUser(userName){
    this.service.deleteUser(userName).subscribe(
      (res: any) => {
        this.toastr.success(`User ${res.userName} has been deleted!`, 'Delete successfull')
        this.getUsers()
      },
      (err: any) => {
        console.log(err)
        if(err.error.message){
          this.toastr.error(`${err.error.message}`, 'Error')
        }
        else{
          this.toastr.error("Cannot delete user with items in shopping bag", 'Error')
        }
      }
    )
  }
  register(){
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Password,
      Role: this.formModel.value.Role,
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.toastr.success(`User ${this.formModel.value.UserName} added!`,'Successfully registered!')
          this.formModel.reset()
          this.formModel.markAsUntouched()
          this.getUsers()
        }
        else{
          res.errors.forEach(element => {
            if(element.code == 'DuplicateUserName'){
              this.toastr.error('Username taken','Registration failed!')
            }
            else
              this.toastr.error(`${element.description}`, 'Registration failed!')
          });
        }
        console.log(res)
      }
    )
  }
  comparePasswords(){
    if(this.formModel.value.Password == this.formModel.value.ConfirmPassword){
      this.passwordsMatch = true
    }
    else
      this.passwordsMatch = false
  }
  confirmZone(){
    this.detailsPhoneZone = 2
  }
  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0)

    var reader = new FileReader()
    reader.onload = (event:any) => {
      this.imagePath = event.target.result
    }
    reader.readAsDataURL(this.fileToUpload)

    console.log(this.fileToUpload.name)
    this.newPhone.ImagePath = "../../../assets/" + this.fileToUpload.name

  }
  addPhone(){
    this.detailsPhoneZone = 3
  }
  cancelAddZone(){
    this.detailsPhoneZone = 1
    this.newPhoneDetails.Dimensions = '',
    this.newPhoneDetails.Weight = '',
    this.newPhoneDetails.DisplayType = '',
    this.newPhoneDetails.Resolution = '',
    this.newPhoneDetails.OS = '',
    this.newPhoneDetails.MainCamera = '',
    this.newPhoneDetails.SelfieCamera = '',
    this.newPhoneDetails.Battery = ''
    this.newPhone.Name = ''
    this.newPhone.LaunchDate = ''
    this.newPhone.Price = null
    this.newPhone.ImagePath = ''
    this.newPhone.CompanyId = null
  }
  addDetails(){
    this.service.addPhone(this.newPhone).subscribe(
      (res: any) => {
        this.toastr.success(`Phone ${res.name} added in the shop`,'Add succeddfull!')
        this.newPhone.Name = ''
        this.newPhone.LaunchDate = ''
        this.newPhone.Price = null
        this.newPhone.ImagePath = ''
        this.newPhone.CompanyId = null
        this.detailsPhoneZone = 3
        this.imagePath = "../../../assets/default-image.png"
      }
    )
    
    setTimeout(()=> this.service.addPhoneDetails(this.newPhoneDetails).subscribe(
      res => {
        this.getPhonesWithCompanies()
        this.toastr.success(`Phone details added`,'Add succeddfull!')
        this.newPhoneDetails.Dimensions = '',
        this.newPhoneDetails.Weight = '',
        this.newPhoneDetails.DisplayType = '',
        this.newPhoneDetails.Resolution = '',
        this.newPhoneDetails.OS = '',
        this.newPhoneDetails.MainCamera = '',
        this.newPhoneDetails.SelfieCamera = '',
        this.newPhoneDetails.Battery = ''
        this.detailsPhoneZone = 1
      }
    ), 600)
  }
  addCompany(){
    this.service.addCompany(this.newCompany).subscribe(
      (res: any) => {
        this.toastr.success(`Company ${res.name} added successfully!`,'Add successfull!')
        this.newCompany.Name = ''
        this.newCompany.Description = ''
        this.newCompany.LaunchDate = ''
      }
    )
  }
  deletePhone(idPhone){
    this.service.deletePhone(idPhone).subscribe(
      (res: any) => {
        this.toastr.success(`Phone ${res.name} has been deleted from the shop`,'Delete successfull!')
        this.getPhonesWithCompanies()
      },
      err => {
        this.toastr.error('Cannot delete a phone if it is in a shopping bag','Error!')
      }
    )
  }
  edit(idPhone, name, company, launchDate, price){
    this.currentEditPhone = idPhone
    this.updatePhone.newName = name 
    this.updatePhone.newCompany = company 
    this.updatePhone.newLaunchDate = launchDate 
    this.updatePhone.newPrice = price
  }
  cancelEdit(){
    this.currentEditPhone = null
    this.updatePhone.newName = '' 
    this.updatePhone.newCompany = '' 
    this.updatePhone.newLaunchDate = '' 
    this.updatePhone.newPrice = ''
  }
  update(){
    this.service.updatePhone(this.currentEditPhone, this.updatePhone).subscribe(
      (res: any) => {
        this.getPhonesWithCompanies()
        this.toastr.success(`Phone ${res.name} has been successfully updated`,'Update successfull!')
        this.currentEditPhone = null
        this.updatePhone.newName = '' 
        this.updatePhone.newCompany = '' 
        this.updatePhone.newLaunchDate = '' 
        this.updatePhone.newPrice = ''
      }
    )
  }
  delivered(idHistoric){
    this.service.deliver(idHistoric).subscribe(
      res => {
        this.toastr.success('Delivered!')
        this.getHistoric()
      }
    )
  }
}
