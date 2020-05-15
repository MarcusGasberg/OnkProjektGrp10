import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { tap, map, filter } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

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
    public oidcSecurityService: OidcSecurityService,
    public httpClient: HttpClient
  ) {}

  ngOnInit() {
    this.oidcSecurityService
      .checkAuth()
      .subscribe((auth) => console.log('App is authenticated', auth));

    this.isLoggedIn$ = this.oidcSecurityService.isAuthenticated$;
    this.name$ = this.oidcSecurityService.userData$.pipe(
      filter((userdata) => userdata != null),
      map((userdata) => userdata.name)
    );
  }

  login(): void {
    this.oidcSecurityService.authorize();
  }

  logout(): void {
    this.oidcSecurityService.logoff();
  }

  account(): void {}

  testApi() {
    this.httpClient
      .get('https://localhost:5001/weatherforecast')
      .subscribe((wf) => console.log(wf));
  }
}
