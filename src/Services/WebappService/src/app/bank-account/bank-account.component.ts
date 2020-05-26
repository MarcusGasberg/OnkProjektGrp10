import { Component, OnInit, Input } from '@angular/core';
import { BankAccount } from '../models/account';

@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss'],
})
export class BankAccountComponent implements OnInit {
  constructor() {}
  @Input() Account: BankAccount;

  ngOnInit(): void {}
}
