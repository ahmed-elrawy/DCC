import { Component, OnInit, Inject } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_services/Auth.service';
import { User } from './_models/User';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/platform-browser';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { SignalRService } from './_services/signalR.service';
 


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  lodaingHome = true ;
 jwthelper = new JwtHelperService();
 constructor(private http: HttpClient,
  private authService: AuthService,
  private signalr: SignalRService ,
  private router: Router,@Inject(DOCUMENT) private document: Document ) {
    console.log(this.signalr.notification);
 }
  token = localStorage.getItem('token');

 ngOnInit() {
  this.signalr.startConnection();

    const token = localStorage.getItem('token');
   const user: User =  JSON.parse(localStorage.getItem('currentUser')) ;
   if (token) {
      this.authService.decodedToken = this.jwthelper.decodeToken(token);
   }
   if (user) {
     this.authService.currentUser = user;
     this.authService.changeMemberPhoto(user.photoUrl);
   }
   this.router.events.subscribe((res) => { 
     if(this.router.url === "/home"){
          this.document.body.classList.add('home');
     }
     else{
      this.document.body.classList.remove('home');
     }
 })
}
 
 }
