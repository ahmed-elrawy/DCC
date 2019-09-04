import { Injectable, Inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs';
 import { environment } from 'src/environments/environment';
import { Base_Remote_ApiUrl } from 'src/Config/defaultss.config';

@Injectable()
export class ApiUrlInterceptor implements HttpInterceptor {
constructor(@Inject(Base_Remote_ApiUrl) private baseRemoteApiUrl: string) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({url: this.prepareUrl(req.url)});
    return next.handle(req);
  }

  private isAbsoluteUrl(url: string): boolean {
    const absolutePattern = /^https?:\/\//i;
    return absolutePattern.test(url);
  }

  private prepareUrl(url: string): string {
    url = this.isAbsoluteUrl(url) ? url : this.baseRemoteApiUrl + '/' + url;
    return url.replace(/([^:]\/)\/+/g, '$1');
  }
}
