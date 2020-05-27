import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  @Input() IsLoggedIn: boolean;
  @Input() Name: string;
  @Output() Login = new EventEmitter();
  @Output() Logout = new EventEmitter();
  @Output() Account = new EventEmitter();
  @Output() TestApi = new EventEmitter();
  @Output() Home = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  loginClick(): void {
    this.Login.emit();
  }

  logoutClick(): void {
    this.Logout.emit();
  }

  accountClick(): void {
    this.Account.emit();
  }

  testApiClick(): void {
    this.TestApi.emit();
  }

  homeClick(): void {
    this.Home.emit();
  }
}
