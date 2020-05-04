import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private manager = new UserManager(this.getClientSettings());
  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  private user: User | null;
  public authNavStatus$ = this.authNavStatusSource.asObservable();

  constructor() {
    this.manager.getUser().then((user) => {
      this.user = user;
      this.authNavStatusSource.next(this.isAuthenticated());
    });
  }

  public login() {
    this.manager.signinSilent();
  }

  public register(username: string, password: string, email: string) {}

  public getClientSettings(): UserManagerSettings {
    return {
      authority: 'http://localhost:5000',
      client_id: 'angular_spa',
      redirect_uri: 'http://localhost:4200/auth-callback',
      response_type: 'id_token token',
      scope: 'openid profile email user_api',
    };
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }
}
