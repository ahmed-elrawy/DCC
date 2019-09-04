import { User } from 'src/app/_models/User';
import { AuthService } from './../_services/Auth.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { faCoffee, faUserMd, faUserCog,faHistory } from '@fortawesome/free-solid-svg-icons';
import { SignalRService } from '../_services/signalR.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  faUserMd = faUserMd;
  faUserCog = faUserCog ;
  historyIcon = faHistory ;
  photoUrl: string ;
  visible: boolean ;

  constructor(public auth: AuthService, private alert: AlertifyService, 
    private sigR : SignalRService,
    private router: Router) { 
      this.sigR.flag.subscribe((r) =>{ this.visible = r , console.log(r)} )
     }
  model: any = {} ;
  ngOnInit() {
    if (this.loggedIn()) {
      console.log(this.auth.getUsername());
    }
    this.auth.currentPhotourl.subscribe(photourl => this.photoUrl = photourl);
  }

Login() {
  this.auth.login(this.model).subscribe((x) => {
    this.alert.success('Welcome Back ' + this.auth.getUsername());
   }, error =>  {
    this.alert.error(error);
   }, () => {
    this.router.navigate(['/lists']);
   }
  );
}
loggedIn() {
   return this.auth.loggedIn();
}
logout() {
this.auth.logout();
}
isDoctor(){
   
   if (this.auth.currentUser.typeOfUser === "Doctors"){
    return true ;
  }else {
    return false ;
  }
}
viewMessage(){
  this.sigR.setPadg(false);
}
}
