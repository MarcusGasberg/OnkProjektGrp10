import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Stock } from '../models/stock';

@Component({
  selector: 'app-stock-list-item',
  templateUrl: './stock-list-item.component.html',
  styleUrls: ['./stock-list-item.component.scss'],
})
export class StockListItemComponent implements OnInit {
  @Input() stock: Stock;
  @Output() selected = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  public clicked() {
    this.selected.emit(this.stock.id);
  }
}
