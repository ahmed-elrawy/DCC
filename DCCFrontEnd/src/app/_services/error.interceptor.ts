import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
        catchError(error => {
            if ( error instanceof HttpErrorResponse) {
                if ( error.status === 401) {
                    return throwError(error.statusText);
                }
                if ( error.status === 400) {
                     return throwError(error.error);
                }

                const applicationerror = error.headers.get('Application-Error');
                if ( applicationerror) {
                    console.error(applicationerror);
                    return throwError(applicationerror);
                }
                const serverError = error.error;
                let modelStateError = '' ;
                if (serverError && typeof serverError === 'object') {
                    for (const key in serverError) {
                        if (serverError[key]) {
                            modelStateError += serverError[key] + '\n';
                        }
                    }
                }
                return throwError(modelStateError || serverError || 'Server Error');
            }
        })
    ) ;
}
}
export const ErrorInterceptorProvider = {
provide: HTTP_INTERCEPTORS ,
useClass: ErrorInterceptor,
multi: true ,
} ;
