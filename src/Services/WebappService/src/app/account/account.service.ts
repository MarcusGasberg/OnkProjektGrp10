import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { filter, map, tap, finalize, catchError } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BankAccount } from '../models/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  bankAccount$: Subject<BankAccount> = new Subject();
  loading$: BehaviorSubject<boolean> = new BehaviorSubject(false);

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

  get Loading$(): Observable<boolean> {
    return this.loading$;
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
    this.loading$.next(true);

    return this.httpClient
      .post<BankAccount>(`${environment.bankUrl}/customer`, account)
      .pipe(finalize(() => this.loading$.next(false)));
  }

  fetchBankAccount() {
    this.loading$.next(true);

    return this.httpClient
      .get<BankAccount>(`${environment.bankUrl}/customer`)
      .pipe(
        tap((ba: BankAccount) => this.bankAccount$.next(ba)),
        finalize(() => this.loading$.next(false))
      );
  }

  putBankAccount(account: BankAccount) {
    this.loading$.next(true);

    return this.httpClient
      .put<BankAccount>(`${environment.bankUrl}/customer`, account)
      .pipe(
        tap((ba) => this.bankAccount$.next(ba)),
        finalize(() => this.loading$.next(false))
      );
  }

  addCredits(amount: number) {
    this.loading$.next(true);

    return this.httpClient
      .post(`${environment.bankUrl}/customer/credits`, amount)
      .pipe(
        tap(() => this.fetchBankAccount().subscribe()),
        finalize(() => this.loading$.next(false))
      );
  }
}
