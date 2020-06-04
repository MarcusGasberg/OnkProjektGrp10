import { Component, OnInit } from '@angular/core';
import { Stock } from '../models/stock';
import { StockService } from '../stock.service';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public stocks = [
    { change: 5, name: 'Stock One' } as Stock,
    { change: -3, name: 'Stock Two' } as Stock,
    { change: -2, name: 'Stock Three' } as Stock,
    { change: 6, name: 'Stock Four' } as Stock,
    { change: 4.1, name: 'Stock Five' } as Stock,
    { change: -2.6, name: 'Stock Six' } as Stock,
  ];

  constructor() {}

  ngOnInit(): void {}
}
