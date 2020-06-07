import { Component, OnInit } from '@angular/core';
import { Stock } from '../models/stock';
import {
  FormControl,
  Validators,
  FormGroup,
  FormBuilder,
} from '@angular/forms';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AccountService } from '../account/account.service';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-sellstock',
  templateUrl: './sellstock.component.html',
  styleUrls: ['./sellstock.component.scss'],
})
export class SellstockComponent implements OnInit {
  public stocks: Stock[];
  public name = '';

  public stockFrom: FormGroup;

  public selectedStock = null;

  public isBuying = false;
  public isSelling = true;
  public amount: string;

  public numberFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern('[0-9]+'),
  ]);

  constructor(
    private formBuilder: FormBuilder,
    private httpClient: HttpClient,
    private accountService: AccountService
  ) {
    this.stockFrom = formBuilder.group({
      amount: [null, Validators.required],
    });
    combineLatest([
      httpClient.get<Stock[]>(`${environment.stockMarketController}/userstock`),
      accountService.Name$,
    ]).subscribe(([res, name]) => {
      console.log(res);
      this.stocks = new Array<Stock>();
      res.forEach((stock) => {
        const amount = stock.seller.find((s) => s.sellerId === name)
          .sellingAmount;
        this.stocks.push({
          id: stock.id,
          name: stock.name,
          change: 0,
          value: stock.historicPrice[0].price,
          owned: amount,
        } as Stock);
      });
      this.selectedStock = this.stocks[0];
    });
  }

  public sell() {
    this.httpClient
      .post(`${environment.stockMarketController}/sell`, {
        StockName: this.selectedStock.name,
        // tslint:disable-next-line: radix
        Number: this.stockFrom.value.amount,
      })
      .subscribe();
    this.stockFrom.reset();
  }

  public selected(id: string) {
    this.selectedStock = this.stocks.find((s) => s.id === id);
    this.stockFrom.reset();
  }

  ngOnInit(): void {}
}
