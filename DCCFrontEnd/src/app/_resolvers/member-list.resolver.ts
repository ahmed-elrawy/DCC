import { catchError } from 'rxjs/operators';
import { UserService } from 'src/app/_services/user.service';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/User';
import { Observable, of } from 'rxjs';
@Injectable()
export class MemberListResolver implements Resolve<User[]> {
    pageNumber =1 ;
    pageSize = 5 ;
 
     constructor(private userService: UserService, private rout: Router , private alert: AlertifyService) {
     }
     resolve(route: ActivatedRouteSnapshot ):  Observable<User[]>  {
         return this.userService.getUserPaging(this.pageNumber,this.pageSize).pipe(
             catchError(err => {
                 this.alert.error('problem retrive data ') ;
                 this.rout.navigate(['/home']);
                 return of(null);
             })
         );
     }
}
