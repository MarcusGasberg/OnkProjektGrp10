import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public password: string;
  public username: string;
  public loading: boolean = false;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {}

  public login() {
    this.auth.login();
  }
}
