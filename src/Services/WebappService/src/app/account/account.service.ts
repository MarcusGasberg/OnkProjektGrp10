import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { filter, map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BankAccount } from '../models/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(
    private httpClient: HttpClient,
    private oidcSecurityService: OidcSecurityService
  ) {}

  get Name$(): Observable<string> {
    return this.oidcSecurityService.userData$.pipe(
      filter((userdata) => userdata != null),
      map((userdata) => userdata.name)
    );
  }

  get IsLoggedIn$(): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$;
  }

  get BankAccount$(): Observable<BankAccount> {
    return this.httpClient.get<BankAccount>(`${environment.bankUrl}/customer`);
  }

  checkAuth() {
    return this.oidcSecurityService.checkAuth();
  }

  login(): void {
    this.oidcSecurityService.authorize();
  }

  logout(): void {
    this.oidcSecurityService.logoff();
  }
}
