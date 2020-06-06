import { Component, OnInit } from '@angular/core';
import { Stock } from '../models/stock';
import { FormControl, Validators } from '@angular/forms';
import Axios from 'axios';

@Component({
  selector: 'app-buy-sell-stock',
  templateUrl: './buy-sell-stock.component.html',
  styleUrls: ['./buy-sell-stock.component.scss'],
})
export class BuySellStockComponent implements OnInit {
  public stocks: Stock[];

  public selectedStock = null;

  public isBuying = true;
  public isSelling = false;
  public amount: string;

  public numberFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern('[0-9]+'),
  ]);

  constructor() {
    Axios.get('stockmarket/userstock').then((res) => {
      console.log(res.data);
      this.stocks = new Array<Stock>();
      res.data.forEach((stock) => {
        this.selectedStock = {
          name: stock.Name,
          change: 0,
          value: 5000,
        } as Stock;
        this.stocks.push({
          name: stock.Name,
          change: 0,
          value: 5000,
        } as Stock);
      });
    });
  }

  public buy() {
    Axios.post('stockbroker/Purchase', {
      StockName: this.selectedStock.name,
      // tslint:disable-next-line: radix
      Number: parseInt(this.amount),
    });
  }

  ngOnInit(): void {}
}
