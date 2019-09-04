import { catchError } from 'rxjs/operators';
import { UserService } from 'src/app/_services/user.service';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { Message } from '../_models/Message';
import { AuthService } from '../_services/Auth.service';
@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer ='Inbox';
    constructor(private userService: UserService, 
        private authService: AuthService ,
        private rout: Router, private alert: AlertifyService) {
    }
    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        console.log('form resolver', this.authService.decodedToken.nameid)
        return this.userService.getMessages(this.authService.decodedToken.nameid,
            this.pageNumber, this.pageSize,this.messageContainer).pipe(
                
            catchError(err => {
                this.alert.error('problem retrive messages ');
                this.rout.navigate(['/home']);
                return of(null);
            })
        );
    }
}
