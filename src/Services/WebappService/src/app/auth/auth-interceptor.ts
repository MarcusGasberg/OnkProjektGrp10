import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpHeaders,
} from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(public oidcSecurityService: OidcSecurityService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.oidcSecurityService.getToken();
    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });
    const authReq = req.clone({ headers });
    return next.handle(authReq);
  }
}
