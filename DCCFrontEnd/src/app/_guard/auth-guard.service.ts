import { AlertifyService } from './../_services/alertify.service';
import { CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { promise } from 'protractor';
import { AuthService } from '../_services/Auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

constructor(private authService: AuthService, private router: Router, private alert: AlertifyService) { }
canActivate(): boolean {
   if (!this.authService.loggedIn()) {
   this.authService.logout();
   this.alert.error('You Sall Not Path !');
   this.router.navigate(['/home']);
  return false;
}
return true;

}
}
