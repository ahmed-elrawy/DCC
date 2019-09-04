import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import {   of, interval, Subscription, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

constructor(private http: HttpClient, private router: Router) { }
jwtHelper = new JwtHelperService();
decodedToken: any ;
logged: boolean;
currentUser: User;
photoUrl = new BehaviorSubject<string>('../../assets/user.png');
currentPhotourl = this.photoUrl.asObservable();
loggedIn$ = new BehaviorSubject<boolean>(this.logged);
private refreshSubscription$: Subscription;

changeMemberPhoto(photoUrl: string) {
this.photoUrl.next(photoUrl)
}

login(model: any) {
  return this.http.post('/api/auth/login' , model)
  .pipe(
    map((response: any) => {
      const user = response ;
      if ( user) {
      localStorage.setItem('token', user.token['result']);
      localStorage.setItem('currentUser', JSON.stringify(user.user));
      this.currentUser = user.user
      this.changeMemberPhoto(this.currentUser.photoUrl);  
      }
    })
  );
}

register(user: User) {
  return this.http.post(   '/api/auth/register', user) ;
}

loggedIn() {
const token = localStorage.getItem('token');
 return !this.jwtHelper.isTokenExpired(token);
}


setLoggedIn(value: boolean) {
  this.loggedIn$.next(value);
  this.logged = value;
}


logout() {
   localStorage.removeItem('token');
   localStorage.removeItem('currentuser')
   this.setLoggedIn(false);
   this.currentUser = null ;
   this.decodedToken = null ;
   this.router.navigate(['/home']);

 }


public getToken(): string {
  return localStorage.getItem('token');
}

getUsername() {
  const token = localStorage.getItem('token');
  this.decodedToken = this.jwtHelper.decodeToken(token);
  return this.decodedToken.unique_name ;

}

// private scheduleRefresh(): void {
//   let tok = this.jwtHelper.decodeToken(this.getToken());
//  console.log(tok);
// this.refreshSubscription$ = of(tok)
//   .pipe(
//     // refresh every half the total expiration time
//     flatMap(tokens => interval(((tok.exp - tok.iat) / 2) * 1000))
//     //,
//     //flatMap(() =>this.refreshLogin())
//   ).subscribe(() => {
//   //  console.log("scheduleRefresh done")

//     if (this.rememberMe) {

//       return this.refreshLogin().subscribe()
//     }
//     else {
//       this.router.navigate(['/acount/login']);
//       return this.logout();
//     }
//   },
// () => {
//   this.router.navigate(['/acount/login']);
//   return this.logout();
// });
// }

// refreshLogin() {
//   let obj = { token: this.getToken, refreshToken: this.refreshToken };
//   return this.http.post( this.refreshUrl, obj).pipe(
//     map((response: LoginResponse) => this.processLoginResponse(response, this.rememberMe)),

//   );
// }
checkEmail(userName){
  return this.http.get(`/api/Auth/checkUserName?userName=${userName}`)
}
}
