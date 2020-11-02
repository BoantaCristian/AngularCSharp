import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AccidentService } from 'src/app/services/accident.service';

@Component({
  selector: 'app-add-agent-dialog',
  templateUrl: './add-agent-dialog.component.html',
  styleUrls: ['./add-agent-dialog.component.css']
})
export class AddAgentDialogComponent implements OnInit {
  mismatch: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private service: AccidentService, private toastr: ToastrService) { }

  agentForm = this.formBuilder.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required],
    Role: 'Agent'
  })

  ngOnInit() {
  }

  matchPasswords(){
    if(this.agentForm.value.Password == this.agentForm.value.ConfirmPassword)
      this.mismatch = false
    else
      this.mismatch = true
  }

  addAgent(){
    var body = {
      UserName: this.agentForm.value.UserName,
      Email: this.agentForm.value.Email,
      Password: this.agentForm.value.Password,
      Role: this.agentForm.value.Role,
      Supervizor: this.data
    }
    this.service.register(body).subscribe(
      (res: any) => {
        if(res.succeeded){
          this.toastr.success(`User ${body.UserName} registered with success`, "Success!")
          this.agentForm.reset()
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
      err =>{
        console.log(err)
      }
    )
  }
}
