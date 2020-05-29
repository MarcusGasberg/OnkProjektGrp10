import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BankAccount } from '../models/account';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { VirtualTimeScheduler } from 'rxjs';

@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss'],
})
export class BankAccountComponent implements OnInit {
  form: FormGroup;
  editing = false;

  constructor(private fb: FormBuilder) {}
  @Input() Account: BankAccount;
  @Output() AddCredits = new EventEmitter();
  @Output() Save = new EventEmitter();

  ngOnInit(): void {
    this.form = this.fb.group({
      registrationNumber: [
        this.Account.registrationNumber,
        Validators.required,
      ],
    });
    this.form.disable();
  }

  edit(): void {
    this.editing = !this.editing;
    if (this.editing) {
      this.form.enable();
    } else {
      this.form.disable();
    }
  }

  save(): void {
    this.Save.emit(this.form.value as BankAccount);
    this.form.markAsPristine();
  }

  addCredits(): void {
    this.AddCredits.emit();
  }
}
