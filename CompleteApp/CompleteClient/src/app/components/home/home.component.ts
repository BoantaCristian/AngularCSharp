import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { User } from "../../Models/user";
import { Parent } from "../../Models/parent";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(private toastr: ToastrService, private router: Router, private service: UserService) { }
  
  
  allChildren: Object;
  selectedChild: any
  selectedParent: any;
  newName: any
  newChild: any
  values: any[] = []
  userDetails: User
  parents: Parent[] = []
  children: any

  logged = false

  ngOnInit() {
      this.getUser()
      this.getAllChildren()
      if(localStorage.getItem('token') != null){
        this.logged = true
      }
      else 
        this.logged = false
      this.getParents()  
  }

  getUser(){
    this.service.getUser().subscribe(
      (user:User)=> {
        this.userDetails = user
      }
    )
  }

  getParents(){
    this.service.getParents().subscribe(
      (res:Parent[]) => {
        this.parents = res
        console.log(this.parents)
      },
      err => {
        console.log(err)
      }
    )
  }

  getAllChildren(){
    this.service.getAllChildren().subscribe(
      res => {
        this.allChildren = res
        console.log(this.allChildren)
      }
    )
  }

  selectParent(id){
    this.selectedParent = id
    this.service.getChildren(id).subscribe(
      (res: any) => {
        this.children = res.children
        console.log(this.children)
      }
    )
  }

  selectParentWithChildren(id){ //selectez parintele fac query si gasesc toti copii si ii sterg si pe ei
    this.selectedParent = id
    this.service.getChildren(id).subscribe(
      (res: any) => {
        this.children = res.children
        console.log('children',this.children)
      }
    )
  }

  selectChild(id){
    this.selectedChild = id
  }

  deleteParentWithChildren(){ //delete parinte si primesc toate idurile si apelez hrrp.delete in for pt fiecare
    this.selectedChild = null
    this.children.forEach(element => {
      console.log(element.id)
      this.service.deleteChild(element.id).subscribe(
        res => {
          this.children = this.getAllChildren()
        }
      )
    });
  }

  deleteChild(){
    this.service.deleteChild(this.selectedChild).subscribe(
      res => {
        this.selectedChild = ''
        this.getAllChildren()
        this.toastr.success(`Child ${this.selectedChild} was deleted`,'Delete successful')
      },
      err => {
        this.toastr.error('Child not selected','Delete failed!')
      }
    )
  }

  addParent(){
    var body = {
      Name: this.newName
    }
    this.service.addParent(body).subscribe(
      res => {
        this.getParents()
        this.newName = ''
        this.toastr.success('New parent added', 'Add successfull')
      }
    )
  }

  addChild(){
    var body = {
      Name: this.newChild,
      Parent: this.selectedParent
    }
    //if(this.newChild == '' //sau null) 
    this.service.addChild(body).subscribe(
      res => {
        this.newChild = ''
        this.getAllChildren()
        this.selectParent(this.selectedParent)
        this.toastr.success('New child added', 'Add successfull')
      },
      err => {
        this.toastr.error('Child name was not given','Add failed!')
      }
    )
  }

  onLogout(){
    localStorage.removeItem('token')
     this.router.navigateByUrl('/user/login')
  }

  onSignin(){
    this.router.navigateByUrl('/user/login')
  }

  onSignup(){
    this.router.navigateByUrl('/user/registration')
  }

}
