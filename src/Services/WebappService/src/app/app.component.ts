import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { tap, map, filter } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AccountService } from './account/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'WebappService';
  isLoggedIn$: Observable<boolean>;
  name$: Observable<string>;

  constructor(
    public httpClient: HttpClient,
    public accountService: AccountService,
    public router: Router
  ) {}

  ngOnInit() {
    this.accountService.checkAuth().subscribe();
    this.isLoggedIn$ = this.accountService.IsLoggedIn$;
    this.name$ = this.accountService.Name$;
    console.log('Environment:', environment);
  }

  login(): void {
    this.accountService.login();
  }

  logout(): void {
    this.accountService.logout();
  }

  account(): void {
    this.router.navigate(['account']);
  }

  testApi(): void {
    this.httpClient
      .post(`${environment.testApiUrl}/customer`, {})
      .subscribe((wf) => console.log(wf));
  }
}
