import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  logged
  userDetails
  parents
  children
  uncles
  fullUncles
  constructor(private router: Router, private service: UserService) { }

  ngOnInit() {
    this.isLogged()
    this.getUser()
    this.getParents()
    this.getUncles()
    //this.getFullUncles()
  }

  logout(){
    localStorage.removeItem('token')
    this.router.navigateByUrl('/user/login')
  }

  getUser(){
    this.service.getUser().subscribe(
      res => {
        this.userDetails = res
      },
      err => {
        console.log(err)
      }
    )
  }

  getParents(){
    this.service.getParents().subscribe(
      res =>{
        this.parents = res
      }
    )
  }

  getUncles(){
    this.service.getUncles().subscribe(
      (res:any) =>{
        this.uncles = res
        console.log(res)
      }
    )
  }

  getFullUncles(){
    this.service.getFullUncles().subscribe(
      res =>{
        this.fullUncles = res
        console.log(res)
      }
    )
  }

  getChildrenOfParent(id){
    this.service.getChildrenOfParent(id).subscribe(
      (res:any) => {
        this.children = res.children
      }
    )
  }

  isLogged(){
    if(localStorage.getItem('token') != null){
      this.logged = true
    }
    else {
      this.logged = false
    }
  }

}
