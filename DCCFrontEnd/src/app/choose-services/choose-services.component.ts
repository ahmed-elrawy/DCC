import { Component, OnInit, AfterContentInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-choose-services',
  templateUrl: './choose-services.component.html',
  styleUrls: ['./choose-services.component.css']
})
export class ChooseServicesComponent implements OnInit, AfterContentInit {
  ngAfterContentInit(): void {
    this.loading = false 
   }

  constructor(private rout : Router) { }
  loading = true ;
  ngOnInit() {
  }
goToDoctors(){
this.rout.navigate(['/doctors'])
}
}
