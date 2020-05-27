import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { filter, map, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BankAccount } from '../models/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  bankAccount$: Subject<BankAccount> = new Subject();

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

  getBankAccount$(): Observable<BankAccount> {
    return this.bankAccount$;
  }

  setBankAccount(account: BankAccount): void {
    this.bankAccount$.next(account);
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

  registerBank(account: BankAccount) {
    return this.httpClient.post<BankAccount>(
      `${environment.bankUrl}/customer`,
      account
    );
  }

  fetchBankAccount() {
    return this.httpClient
      .get<BankAccount>(`${environment.bankUrl}/customer`)
      .pipe(tap((ba) => this.bankAccount$.next(ba)));
  }
}
