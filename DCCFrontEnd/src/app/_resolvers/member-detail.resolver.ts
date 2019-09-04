import { catchError } from 'rxjs/operators';
import { UserService } from 'src/app/_services/user.service';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/User';
import { Observable, of } from 'rxjs';
@Injectable()
export class MemberDetailResolver implements Resolve<User> {
     constructor(private userService: UserService, private rout: Router , private alert: AlertifyService) {
     }
     resolve(route: ActivatedRouteSnapshot ):  Observable<User>  {
         return this.userService.getUser(route.params['id']).pipe(
             catchError(err => {
                 this.alert.error('problem retrive data ') ;
                 this.rout.navigate(['/doctors']);
                 return of(null);
             })
         );
     }
}
