import { Component, OnInit } from '@angular/core';
import { Stock } from '../models/stock';
import { FormControl, Validators } from '@angular/forms';

@Component( {
  selector: 'app-buy-sell-stock',
  templateUrl: './buy-sell-stock.component.html',
  styleUrls: [ './buy-sell-stock.component.scss' ]
} )
export class BuySellStockComponent implements OnInit {

  public stocks = [
    { change: 5, name: 'Stock One' } as Stock,
    { change: -3, name: 'Stock Two' } as Stock,
    { change: -2, name: 'Stock Three' } as Stock,
    { change: 6, name: 'Stock Four' } as Stock,
    { change: 4.1, name: 'Stock Five' } as Stock,
    { change: -2.6, name: 'Stock Six' } as Stock,
    { change: 5, name: 'Stock One' } as Stock,
    { change: -3, name: 'Stock Two' } as Stock,
    { change: -2, name: 'Stock Three' } as Stock,
    { change: 6, name: 'Stock Four' } as Stock,
    { change: 4.1, name: 'Stock Five' } as Stock,
    { change: -2.6, name: 'Stock Six' } as Stock,
    { change: 5, name: 'Stock One' } as Stock,
    { change: -3, name: 'Stock Two' } as Stock,
    { change: -2, name: 'Stock Three' } as Stock,
    { change: 6, name: 'Stock Four' } as Stock,
    { change: 4.1, name: 'Stock Five' } as Stock,
    { change: -2.6, name: 'Stock Six' } as Stock,
    { change: 5, name: 'Stock One' } as Stock,
    { change: -3, name: 'Stock Two' } as Stock,
    { change: -2, name: 'Stock Three' } as Stock,
    { change: 6, name: 'Stock Four' } as Stock,
    { change: 4.1, name: 'Stock Five' } as Stock,
    { change: -2.6, name: 'Stock Six' } as Stock,
  ];

  public selectedStock = { change: 6, name: 'Stock Four', value: 124 } as Stock;

  public isBuying = true;
  public isSelling = false;

  public numberFormControl = new FormControl( '', [
    Validators.required,
    Validators.pattern( '[0-9]+' ),
  ] );

  constructor () { }

  ngOnInit (): void {
  }

}
