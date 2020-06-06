import { Component, OnInit } from '@angular/core';
import { Stock } from '../models/stock';
import {
  FormControl,
  Validators,
  FormGroup,
  FormBuilder,
} from '@angular/forms';
import Axios from 'axios';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-sellstock',
  templateUrl: './sellstock.component.html',
  styleUrls: ['./sellstock.component.scss'],
})
export class SellstockComponent implements OnInit {
  public stocks: Stock[];

  public stockFrom: FormGroup;

  public selectedStock = null;

  public isBuying = false;
  public isSelling = true;
  public amount: string;

  public numberFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern('[0-9]+'),
  ]);

  constructor(private formBuilder: FormBuilder) {
    this.stockFrom = formBuilder.group({
      amount: [null, Validators.required],
    });

    Axios.get(`${environment.stockMarketController}/userstock/2`).then(
      (res) => {
        console.log(res.data);
        this.stocks = new Array<Stock>();
        res.data.forEach((stock) => {
          const amount = stock.seller.find((s) => s.sellerId === '2')
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
      }
    );
  }

  public sell() {
    Axios.post(`${environment.stockMarketController}/sell`, {
      StockName: this.selectedStock.name,
      // tslint:disable-next-line: radix
      Number: this.stockFrom.value.amount,
    });
    this.stockFrom.reset();
  }

  public selected(id: string) {
    this.selectedStock = this.stocks.find((s) => s.id === id);
    this.stockFrom.reset();
  }

  ngOnInit(): void {}
}
