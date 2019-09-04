import { AlertifyService } from './../_services/alertify.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/Auth.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';
import { User } from '../_models/User';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user: User;
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  isDoctor: boolean;
  check;
  specialization = [
    "Bones Doctor"
    ,

    "Heart's Doctor"
    ,
    "Internal Doctor"
    ,
    "Dermatologist"
    , "Feminine Doctor"
    ,

    "Pediatrician"
    ,
  ]

  form: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  @Output() cancelRegister = new EventEmitter();
  constructor(private fb: FormBuilder, private authservice: AuthService,
    private routr: Router,
    private alert: AlertifyService) { }

  ngOnInit() {
    console.log(this.specialization)
    this.bsConfig = {
      containerClass: 'theme-blue'
    }
    this.createForm();
    this.firstFormGroup = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email])
    });
    this.secondFormGroup = new FormGroup({
      password: new FormControl('', Validators.required)
    });

  }
  get email() { return this.firstFormGroup.get('email'); }
  get password() { return this.secondFormGroup.get('password'); }


  cancel() {
    this.cancelRegister.emit(false);
  }
  createForm() {
    this.form = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(14)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(14)]],
      gender: ['male'],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      specialization: ['', Validators.required],
      typeOfUser: ['']

    }, { validator: this.passwordMatches });
  }
  passwordMatches(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true };
  }
  showSpech(event, check) {
    console.log(event, check);
  }
  register() {
    
    console.log(this.form.status)
    if (this.form.valid) {

      this.user = Object.assign({}, this.form.value);
      if(this.check){
        this.user.typeOfUser = "Doctors";
      }else {
        this.user.typeOfUser = "Patient";
      }
      this.authservice.register(this.user).subscribe(() => {
        this.alert.success('Register Success !');
      },
        (error) => {
          this.alert.error(error);
        }, () => {

          this.authservice.login(this.user).subscribe(() => {
            this.routr.navigate(['/doctors']);
          })
        }
      );
    }


  }

}
