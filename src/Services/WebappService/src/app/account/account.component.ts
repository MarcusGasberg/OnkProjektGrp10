import { Component, OnInit } from '@angular/core';
import { AccountService } from './account.service';
import { Observable } from 'rxjs';
import { BankAccount } from '../models/account';
import { MatDialog } from '@angular/material/dialog';
import { BankRegisterComponent } from '../bank-register/bank-register.component';
import { tap, map } from 'rxjs/operators';
import { AddCreditsComponent } from '../add-credits/add-credits.component';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  Name$: Observable<string>;
  BankAccount$: Observable<BankAccount>;
  Loading$: Observable<boolean>;

  constructor(
    private accountService: AccountService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.Name$ = this.accountService.Name$;
    this.BankAccount$ = this.accountService.getBankAccount$();
    this.Loading$ = this.accountService.Loading$;
    this.accountService.fetchBankAccount().subscribe();
  }

  openRegisterDialog() {
    const dialogRef = this.dialog.open(BankRegisterComponent, {
      height: '300px',
      width: '450px',
    });

    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (!result) {
          return;
        }

        this.accountService.registerBank(result as BankAccount).subscribe({
          next: (created) => this.accountService.setBankAccount(created),
        });
      },
    });
  }

  onSave(account: BankAccount): void {
    this.accountService.putBankAccount(account).subscribe();
  }

  onAddCredits() {
    const dialogRef = this.dialog.open(AddCreditsComponent, {
      height: '300px',
      width: '450px',
    });

    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (!result) {
          return;
        }

        this.accountService.addCredits(result).subscribe();
      },
    });
  }
}
