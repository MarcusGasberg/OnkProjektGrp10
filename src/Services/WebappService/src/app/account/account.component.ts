import { Component, OnInit } from '@angular/core';
import { AccountService } from './account.service';
import { Observable } from 'rxjs';
import { BankAccount } from '../models/account';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  Name$: Observable<string>;
  BankAccount$: Observable<BankAccount>;

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.Name$ = this.accountService.Name$;
    this.BankAccount$ = this.accountService.BankAccount$;
  }

  registerBank() {}
}
