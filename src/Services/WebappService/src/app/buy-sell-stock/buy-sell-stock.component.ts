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

@Component({
  selector: 'app-buy-sell-stock',
  templateUrl: './buy-sell-stock.component.html',
  styleUrls: ['./buy-sell-stock.component.scss'],
})
export class BuySellStockComponent implements OnInit {
  public stocks: Stock[];

  public stockFrom: FormGroup;

  public selectedStock = null;

  public isBuying = true;
  public isSelling = false;
  public amount: string;

  public numberFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern('[0-9]+'),
  ]);

  constructor(
    private formBuilder: FormBuilder,
    private httpClient: HttpClient
  ) {
    this.stockFrom = formBuilder.group({
      amount: [null, Validators.required],
    });

    httpClient
      .get<Stock[]>(`${environment.stockMarketController}`)
      .subscribe((res) => {
        console.log(res);
        this.stocks = new Array<Stock>();
        res.forEach((stock) => {
          this.stocks.push({
            name: stock.name,
            id: stock.id,
            change: 0,
            value: stock.historicPrice[0].price,
          } as Stock);
        });
        this.selectedStock = this.stocks[0];
      });
  }

  public buy() {
    this.httpClient
      .post(`${environment.stockBrokerController}/Purchase`, {
        StockName: this.selectedStock.name,
        // tslint:disable-next-line: radix
        Number: this.stockFrom.value.amount,
      })
      .subscribe();
    this.stockFrom.reset();
  }

  public selected(id: string) {
    this.selectedStock = this.stocks.find((s) => s.id === id);
  }

  ngOnInit(): void {}
}
